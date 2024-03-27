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

using System;
using System.Threading;
using System.ComponentModel;
 
class Program
{
  static BackgroundWorker _bw;
 
  static void Main()
  {
    _bw = new BackgroundWorker
    {
      WorkerReportsProgress = true,
      WorkerSupportsCancellation = true
    };

    _bw.DoWork += bw_DoWork;
    _bw.ProgressChanged += bw_ProgressChanged;
    _bw.RunWorkerCompleted += bw_RunWorkerCompleted;

    _bw.RunWorkerAsync ("Hello to worker");
 
    Console.WriteLine ("Press Enter in the next 5 seconds to cancel");
    Console.ReadLine();
    if (_bw.IsBusy) _bw.CancelAsync();
    Console.ReadLine();
  }
 
  static void bw_DoWork (object sender, DoWorkEventArgs e)
  {
    for (int i = 0; i <= 100; i += 20)
    {
      if (_bw.CancellationPending) { e.Cancel = true; return; }
      _bw.ReportProgress (i);
      Thread.Sleep (1000);      // Just for the demo... don't go sleeping
    }                           // for real in pooled threads!
 
    e.Result = 123;    // This gets passed to RunWorkerCompleted
  }
 
  static void bw_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
  {
    if (e.Cancelled)
      Console.WriteLine ("You canceled!");
    else if (e.Error != null)
      Console.WriteLine ("Worker exception: " + e.Error.ToString());
    else
      Console.WriteLine ("Complete: " + e.Result);      // from DoWork
  }
 
  static void bw_ProgressChanged (object sender, ProgressChangedEventArgs e)
  {
    Console.WriteLine ("Reached " + e.ProgressPercentage + "%");
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
https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/b2117cacc68f84b30c8a6bfd271157513682e734/Test.cs#L24-L31

## Testing embedded links (again use permalink: but select only a set of characxter on the same line)
https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/b2117cacc68f84b30c8a6bfd271157513682e734/Test.cs#LL24C5-L24C20
NOT OK => will only create a link

## Testing refrenced links (use permalink)
https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/main/Test.cs?plain=1#L37-L72
=> will only create a link

## Testing refrenced links directly

**Link to code lines here::**
[Test.cs](Test.cs#L17-L72)

**Authors**
> Xperior Staff

[^1]: The footnote.
