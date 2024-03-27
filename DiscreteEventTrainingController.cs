using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Experior.Core.Loads;
using Experior.Core.Communication.TCPIP;
using Experior.Core.Resources;
using Experior.Core.Routes;
using Experior.Plugin.DiscreteEventTraining;
using Experior.Catalog.DiscreteEvent_Training.Assemblies.Feeder;

namespace Experior.Controller.DiscreteEventTraining
{
    public class DiscreteEventTrainingController : Experior.Core.Controller
    {
        #region Fields

        private readonly object _sendReceiveLock = new object();

        private readonly STXETX _connection;

        private readonly Experior.Core.Timer _timer;

        public event EventHandler OnDataReceivedStxEtx;

        private int messageCounter = 0;

        #endregion

        #region Constructors

        public DiscreteEventTrainingController()
            : base(nameof(DiscreteEventTrainingController))
        {
            Core.Environment.Scene.OnLoaded += Scene_OnLoaded;
            OnDataReceivedStxEtx += DataReceivedStxEtx;

            _timer = new Core.Timer(10);
            _timer.OnElapsed += _timer_OnElapsed;

            //Create connection
            var connection = Core.Communication.Connection.Items.FirstOrDefault(item => item.Key.Equals("WMS"));
            if (!connection.Equals(default(KeyValuePair<STXETX, string>)))
            {
                _connection = (STXETX)connection.Value;
                _connection.OnTelegramReceived += Ascii_TelegramReceived;
            }

        }

        private void _timer_OnElapsed(Core.Timer sender)
        {
            Log.Write($"Timer expiered - releasing the stopped load");

            Load load = (Load)Experior.Core.Loads.Load.Items.FirstOrDefault(l => l.Stopped);

            if (load != null)
                load.Release();
        }

        #endregion

        #region Properties 
        public override ImageSource Logo => EmbeddedResource.GetImage("BasicTrainingController");

        #endregion

        #region Override Methods


        /// <summary>
        /// Handle loads arriving at action points
        /// </summary>
        /// <param name="node">The current action point</param>
        /// <param name="load">The current load on this action point</param>
        protected override void Arriving(INode node, Load load)
        {
            //base.Arriving(node, load);
            var actionPoint = node.Name;
            Log.Write($"Arriving(INode {node}, Load {load})");

            switch (actionPoint)
            {
                case "AP_SelectChute":
                    var chutes = new List<string> { "AP_Chute1", "AP_Chute2", "AP_Chute3", "AP_Chute4", "AP_Chute5", "AP_Chute6" };
                    var randomDestination = chutes[Core.Environment.Random.Next(0, chutes.Count)];
                    Log.Write($"Random Destination = {randomDestination}");
                    var requestTelegram = $"PlcToWms|Request|LoadDestination|{++messageCounter}|{load.Identification}|{actionPoint}|";
                    StxEtxSend(requestTelegram);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Reset called from Experior
        /// </summary>
        protected override void Reset()
        {
            base.Reset();
            FeedersSubscriptionOnLoadAdded();
        }

        /// <summary>
        /// Dispose called
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            FeedersSubscriptionOnLoadAdded(dispose: true);

            if (_connection != null)
            {
                _connection.OnTelegramReceived += Ascii_TelegramReceived;
            }

            _timer.OnElapsed -= _timer_OnElapsed;
            _timer.Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Experior call when the scene is loaded
        /// </summary>
        private void Scene_OnLoaded()
        {
            Reset();
            Core.Environment.Time.TargetTimeScale = 1.0f;
            Core.Environment.Random.Seed = 2183;
            Core.Environment.Scene.Shadow = 1;
            Core.Communication.Internal.AddListener(this, InternalMessageReceived);

            //Notice: To add a listener in a Plugin we need a singleton instance
            DiscreteEventTrainingPlugin.Instance.AddListener();

            //Core.Environment.UI.MessageBox.Show("Test"); //Test
        }


        /// <summary>
        /// Event when data is alidated by  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedStxEtx(object sender, EventArgs e)
        {
            Core.Environment.InvokeIfRequired(() =>
            {
                Log.Write($"Data received");
            });
        }

        /// <summary>
        /// Send string telegram
        /// </summary>
        /// <param name="telegram"></param>
        private void StxEtxSend(string telegram)
        {
            _connection?.Send(telegram);
            Log.Write($"Sender: {Name} Tx telegram: {telegram}");
        }

        /// <summary>
        /// Receive a telegram (byte array) on the STX/ETX connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void Ascii_TelegramReceived(Core.Communication.Connection connection, string message)
        {
            lock (_sendReceiveLock)
            {
                Core.Environment.InvokeIfRequired(() =>
                {
                    Log.Write($"Receive message: [{message}]");

                    //Simple Protocol: "Direction|Type|Command|MessageCounter|LoadId|DecisionPoint|DestinationPoint"
                    var parts = message.Split('|');

                    var direction = parts[0];
                    var type = parts[1];
                    var command = parts[2];
                    var messageCounter = parts[3];
                    var loadId = parts[4];
                    var decisionPoint = parts[5];
                    var destinationPoint = parts[6];

                    switch (type)
                    {
                        case "Request":
                            switch (command)
                            {
                                case "LoadSpawn": //"WmsToPlc|Request|LoadSpawn|..."
                                    var feeders = Core.Assemblies.Assembly.Items.Where(item => item is CustomFeeder);
                                    if (!feeders.Any())
                                        return;

                                    var feeder = feeders.Cast<CustomFeeder>().FirstOrDefault(f => f.TargetActionPoint.Equals(destinationPoint));
                                    if (feeder == null)
                                        return;

                                    var load = feeder.Feed(Colors.Goldenrod);
                                    uint loadIdentification = Convert.ToUInt32(load.Identification); // 11111;

                                    Log.Write($"Feeder {feeder.Name} created load on {feeder.TargetActionPoint}");

                                    var replyTelegram = $"PlcToWms|Reply|LoadSpawn|{messageCounter}|{loadIdentification}||{destinationPoint}";
                                    StxEtxSend(replyTelegram);
                                    break;
                                default:
                                    Log.Write($"Error: command does not exist: {command}.");
                                    break;
                            }
                            break;
                        case "Reply":
                            switch (command)
                            {
                                case "LoadDestination": //"WmsToPlc|Reply|LoadDestination|..."
                                    Load load = (Load)Experior.Core.Loads.Load.Items.FirstOrDefault(l => l.Identification == loadId);
                                    if (load == null)
                                        return;

                                    load.MoveTo(destinationPoint);
                                    load.Release();

                                    Log.Write($"Moving load {load.Identification} to {destinationPoint}");
                                    break;
                                default:
                                    Log.Write($"Error: command does not exist: {command}.");
                                    break;
                            }
                            break;
                        default:
                            Log.Write($"Error: type does not exist: {type}.");
                            break;
                    }
                });
            }
        }

        /// <summary>
        /// Send byte array telegram
        /// </summary>
        /// <param name="telegram"></param>
        private void StxEtxSend(byte[] telegram)
        {
            _connection?.Send(telegram);
            Log.Write($"Sender: {Name} Tx telegram: {telegram}");
        }

        /// <summary>
        /// Message received on interal communication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reciever"></param>
        /// <param name="message"></param>
        /// <param name="broadcast"></param>
        private void InternalMessageReceived(object sender, object reciever, object message, bool broadcast)
        {
            Log.Communication($"INTERNAL MESSAGE: {message} [{sender} -> {reciever}]");
        }

        private void FeedersSubscriptionOnLoadAdded(bool dispose = false)
        {
            var feeders = Core.Assemblies.Assembly.Items.OfType<Experior.Catalog.DiscreteEvent_Training.Assemblies.Feeder.BasicFeeder>();

            if (feeders == null)
                return;

            foreach (var feeder in feeders)
            {
                Log.Write($"Found Feeder: {feeder.Name} [{this.Name}] ");

                feeder.OnLoadAdded -= Feeder_OnLoadAdded;

                if (dispose)
                {
                    feeder.Dispose();
                    continue;
                }

                feeder.OnLoadAdded += Feeder_OnLoadAdded;
            }
        }

        private void Feeder_OnLoadAdded(object sender, EventArgs e)
        {
            if (!(sender is Load load))
                return;

            Log.Write($"Load {load.Identification} is added to scene [{this.Name}]");
        }

        #endregion
    }

    internal static class EmbeddedResource
    {
        private static EmbeddedImageLoader Images { get; } = new Experior.Core.Resources.EmbeddedImageLoader();
        private static EmbeddedResourceLoader Resource { get; } = new Experior.Core.Resources.EmbeddedResourceLoader();

        public static ImageSource GetImage(string resourceFileName) => Images.Get(resourceFileName);
        public static Experior.Core.Resources.EmbeddedResource GetResource(string resourceFileName) => Resource.Get(resourceFileName);
    }


}