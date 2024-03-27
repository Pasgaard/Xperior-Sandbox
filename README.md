# Xperior-Sandbox

## The wiki
[Wiki](https://experior.wiki/)

## Testing videos

### Animated.gif
![Gif Format](GhostWMS.gif)

### Format .mp4
![VideoMovFormat.mov](https://github.com/Pasgaard/Xperior-Sandbox/blob/main/Media1.mp4)

### Format .mov
![Mov Format](VideoMovFormat.mov)

## Testing various text formatting 

*Emphasize*  _emphasize_

**Strong**  __strong__

==Marked text.==

~~Mistaken text.~~

> Quoted text.

List of things:
- [ ] Unchecked
- [ ] Unchecked
- [x] Checked

- [x] #739
- [ ] https://github.com/octo-org/octo-repo/issues/740
- [ ] Add delight to the experience when all tasks are complete :tada:


## Testing various tables formatting 

**Tables:**

| Column 1     | Column 2 | Column 3      |
|:-------------|:--------:| -------------:|
| left-aligned | centered | right-aligned |

**Another Table**

Item | Value
-------- | -----
Computer | $1600
Phone | $12
Pipe | $1
Some text with a footnote.[^1]


## Testing embedded source code (here C#)

```csharp
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
```

## Testing colored info boxes

> [!NOTE]
> Useful information that users should know, even when skimming content.

> [!TIP]  
> Helpful advice for doing things better or more easily.

> [!IMPORTANT]  
> Key information users need to know to achieve their goal.

> [!WARNING]
> Urgent info that needs immediate user attention to avoid problems.

> [!CAUTION]
> Advises about risks or negative outcomes of certain actions.

## Testing embedded links (use permalink: in code mark a set of lines and on the left side [...] select Create Permalink)
OK
[https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/b2117cacc68f84b30c8a6bfd271157513682e734/Test.cs#L24-L31](https://github.com/Pasgaard/Xperior-Sandbox/blob/d9880727c14f5a7897086b37c2e2381cb7b4c5ee/DiscreteEventTrainingController.cs#L32-L49)

## Testing embedded links (again use permalink: but select only a set of characxter on the same line)
https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/b2117cacc68f84b30c8a6bfd271157513682e734/Test.cs#LL24C5-L24C20
NOT OK => will only create a link

## Testing refrenced links 
https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/main/Test.cs?plain=1#L37-L72
=> will only create a link

## Testing refrenced links directly

**Link to code lines here::**
[DiscreteEventTrainingController.cs](DiscreteEventTrainingController.cs#L17-L72)

**Authors**
> Xperior Staff

[^1]: The footnote.
