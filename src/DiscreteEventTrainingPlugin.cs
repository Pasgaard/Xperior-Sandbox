using Experior.Catalog.DiscreteEvent_Training.Assemblies.Feeder;
using Experior.Core.Resources;
using System;
using System.Linq;
using System.Windows.Media;

namespace Experior.Plugin.DiscreteEventTraining
{
    public class DiscreteEventTrainingPlugin : Experior.Core.Plugin
    {
        #region Fields

        private static DiscreteEventTrainingPlugin _instance; //Singleton

        private Core.Environment.UI.Toolbar.Button _guiButton1;
        private Core.Environment.UI.Toolbar.Button _guiButton2;

        #endregion 

        #region Constructors

        public DiscreteEventTrainingPlugin()
            : base(nameof(DiscreteEventTrainingPlugin))
        {
            _instance = this;

            //Place Images in Icon folder, not the Resources folder
            _guiButton1 = new Core.Environment.UI.Toolbar.Button()
            {
                Text = "Create Load",
                Tooltip = "Create a load at SpawnPoint",
                Image = EmbeddedResource.GetImage("BasicFeeder.png"),
                OnClick = ButtonOnClickBasicFeeder,
                Enabled = true,
            };

            _guiButton2 = new Core.Environment.UI.Toolbar.Button()
            {
                Text = "Create Load",
                Tooltip = "Create a load at SpawnPoint",
                Image = EmbeddedResource.GetImage("CustomFeeder.png"),
                OnClick = ButtonOnClickCustomFeeder,
                Enabled = true,
            };

            // add the buttons to the Model toolbar in a Tab with name "Test Plugin"
            Core.Environment.UI.Toolbar.Add(_guiButton1, "Test Plugin", "Plugin Buttons");
            Core.Environment.UI.Toolbar.Add(_guiButton2, "Test Plugin", "Plugin Buttons");
        }

        #endregion

        #region Properties

        public static DiscreteEventTrainingPlugin Instance => _instance ?? (_instance = new DiscreteEventTrainingPlugin());

        public override ImageSource Logo => EmbeddedResource.GetImage("BasicTrainingPlugin");

        #endregion

        #region Private Methods

        private void ReceiveTelegrams(object sender, object reciever, object message, bool broadcast)
        {
            Core.Environment.InvokeIfRequired(() =>
            {
                Log.Communication($"INTERNAL MESSAGE: {message} [{sender} -> {reciever}]");
            });
        }

        public void AddListener()
        {
            Core.Environment.InvokeIfRequired(() =>
            {
                Experior.Core.Communication.Internal.RemoveListener(Instance);
                Experior.Core.Communication.Internal.AddListener(Instance, ReceiveTelegrams);
            });
        }
                
        private void ButtonOnClickBasicFeeder(object sender, EventArgs args)
        {
            Core.Environment.InvokeIfRequired(() =>
            {
                Log.Write($"Button pressed [{this.Name}]");

                var feeders = Core.Assemblies.Assembly.Items.Where(item => item is BasicFeeder);
                if (!feeders.Any())
                    return;

                foreach (BasicFeeder feeder in feeders.Cast<BasicFeeder>())
                {
                    if (!string.IsNullOrWhiteSpace(feeder.TargetActionPoint))
                    {
                        Log.Write($"Feeder {feeder.Name} creates load on {feeder.TargetActionPoint} [{this.Name}]");
                        feeder.Feed(Colors.Teal);
                        continue;
                    }

                    Log.Warning($"Feeder {feeder.Name} is missing value of TargetActionPoint! [{this.Name}]");
                }

            });
        }

        private void ButtonOnClickCustomFeeder(object sender, EventArgs args)
        {
            Core.Environment.InvokeIfRequired(() =>
            {
                Log.Write($"Button pressed [{this.Name}]");

                var feeders = Core.Assemblies.Assembly.Items.Where(item => item is CustomFeeder);
                if (!feeders.Any())
                    return;

                foreach (CustomFeeder feeder in feeders.Cast<CustomFeeder>())
                {
                    if (!string.IsNullOrWhiteSpace(feeder.TargetActionPoint))
                    {
                        Log.Write($"Feeder {feeder.Name} creates load on {feeder.TargetActionPoint} [{this.Name}]");
                        feeder.Feed(Colors.Goldenrod);
                        continue;
                    }

                    Log.Warning($"Feeder {feeder.Name} is missing value of TargetActionPoint! [{this.Name}]");
                }

            });
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