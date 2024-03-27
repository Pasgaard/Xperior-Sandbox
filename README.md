# Xperior-Sandbox
Test


# My Headline

## To be honest

![Video](Media1.mp4)

<span style="color: #FF69B4;">Why did the tomato turn red?</span>
A few weeks ago bla..bla..

*Emphasize*  _emphasize_

**Strong**  __strong__

==Marked text.==

~~Mistaken text.~~

> Quoted text.

List of things:
- [ ] Unchecked
- [ ] Unchecked
- [x] Checked

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

- [x] #739
- [ ] https://github.com/octo-org/octo-repo/issues/740
- [ ] Add delight to the experience when all tasks are complete :tada:


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

**Authors**
> Xperior Staff

https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/b2117cacc68f84b30c8a6bfd271157513682e734/Test.cs#L24-L31

https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/b2117cacc68f84b30c8a6bfd271157513682e734/Test.cs#LL24C5-L24C20

https://github.com/Pasgaard/Godot-2D-Space-Shooter/blob/main/Test.cs?plain=1#L37-L72

**Link to code lines here::**
[Test.cs](Test.cs#L17-L72)

[^1]: The footnote.
