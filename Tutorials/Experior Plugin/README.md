# Experior Modules: Experior.Core.Plugin

![Plugin](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/2ae16ca8-082b-48f9-aeb1-d496fba9096e=20x20)

## Description
_The only difference between an Experior.Core.Plugin and a normal C# Class Library (dynamic link library) is the access to Experior functionality and an embedded ‘tail’ containing a license-key for the plugin to be used in Experior._ 

1. The Plucing basics
2. Extend the Plugin with GUI Buttons
3. Internal Communication example

## The Plugin basics

* Requirement: A new The Experior Plugin Template.
* Prerequisites: Creation of a new Visual Studio project based on this template.
  
The class **DiscreteEventTrainingPlugin** will be instantiated by Experior during starting process. 
Here below is an example of the default content from the template. 
The plugin holds a static embedded resource and a Logo to be displayed in the GUI.

```csharp
namespace Experior.Plugin.DiscreteEventTraining
{
    public class DiscreteEventTrainingPlugin : Experior.Core.Plugin
    {
        public DiscreteEventTrainingPlugin()
            : base(nameof(DiscreteEventTrainingPlugin))
        {
        }

        public override ImageSource Logo => EmbeddedResource.GetImage("BasicTrainingPlugin");
    }

    internal static class EmbeddedResource
    {
        private static EmbeddedImageLoader Images { get; } = new Experior.Core.Resources.EmbeddedImageLoader();
        private static EmbeddedResourceLoader Resource { get; } = new Experior.Core.Resources.EmbeddedResourceLoader();

        public static ImageSource GetImage(string resourceFileName) => Images.Get(resourceFileName);
        public static Experior.Core.Resources.EmbeddedResource GetResource(string resourceFileName) => Resource.Get(resourceFileName);
    }
}
```


* In the example below two buttons are created. 
* Each button will spawn two types of load in the Scene

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L22-L49

### The methods for the two types 

> [!IMPORTANT]  
> Since the Buttons are activated on the Main Thread, the Invoke on Engine Thread is required:

```csharp
Core.Environment.InvokeIfRequired(() => {//Do stuff here});
```

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L80-L128

## Extend Plugin with custom connections


