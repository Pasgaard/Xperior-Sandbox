# Xperior-Sandbox

## The wiki
[Wiki](https://experior.wiki/)

## Testing videos

### GIF Format (file reference ☑️)
![Gif Format](Videos/GhostWMS.gif)

### MP4 Format (drag and drop directly into this code file ☑️)
https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/1f2bf373-d540-43e1-83de-404d262dbdfe

## Testing various text formatting 

*Emphasize*  _emphasize_

**Strong**  __strong__

==Marked text.==

~~Mistaken text.~~

> Quoted text.

List of things:
- [ ] Unchecked
- [x] Checked
- [ ] https://experior.wiki/
- [ ] Keep fighting :ukraine:

Some text with a footnote.[^1]

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

![TEST UML](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/pasgaard/Xperior-Sandbox/master/test.iuml)

See https://github.com/jonashackt/plantuml-markdown?tab=readme-ov-file

# Testing embedded source code

## Manually embedded source code (here C#)

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

## Embedded link (mark a set of lines in code file. On left side [...] select: Copy Permalink)
https://github.com/Pasgaard/Xperior-Sandbox/blob/d9880727c14f5a7897086b37c2e2381cb7b4c5ee/DiscreteEventTrainingController.cs#L32-L49

## Mark a set of characxter on the same line. On left side [...] select: Copy Permalink - this will only create a link (not embedded!)
https://github.com/Pasgaard/Xperior-Sandbox/blob/141a3871c5b5f3c6015d2dab53abde1c36fb6030/DiscreteEventTrainingController.cs#L24C35-L24C55

## Testing refrenced links directly (./src/DiscreteEventTrainingController.cs#L17-L72)
[DiscreteEventTrainingController.cs](./src/DiscreteEventTrainingController.cs#L17-L72)

**Authors**
> Xperior

[^1]: The footnote.
