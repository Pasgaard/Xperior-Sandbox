# Experior Modules: Experior.Core.Plugin

![Plugin](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/2ae16ca8-082b-48f9-aeb1-d496fba9096e=20x20)

## Description
_The only difference between an Experior.Core.Plugin and a normal C# Class Library (dynamic link library) is the access to Experior functionality and an embedded ‘tail’ containing a license-key for the plugin to be available in Experior._ 

## Topics
1. The Plucing basics
2. Exercise1: Extend the Plugin with GUI Buttons
3. Exercise2: Extend the Plugin with Internal Communication

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

# Exercises

## Exercise 1. Create Buttons in the GUI

![image](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/79d654f2-c3b5-4c3e-83d1-a1442a9f453b)

* In the example below two buttons are created. 
* Each button will spawn a different type of load into the Scene.

> [!TIP]  
> Required component: **Core.Environment.UI.Toolbar.Button**

* The class DiscreteEventTrainingPlugin will first have the Core.Environment.UI.Toolbar.Button added.

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L22-L49

## Examples of the methods for the two load types. 

![image](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/7e93cab8-5637-4367-95ee-8155d11841fb)

* Each button will spawn a load using either a BasicFeeder or a CustomFeeder.
* Each method will search for any matching feeders, adn spawn a load on the feeders attached TargetActionPoint.
* The OnClick method writes to the log.
* Then it finds all Feeders of type BasicFeeder or CustomFeeder.
* Each Feeder will now spawn a Load on it’s TargetActionPoint.
* A warning is written in the log if the TargetActionPoint is missing.

> [!IMPORTANT]  
> Since the Buttons are activated on the Main Thread, an Invoke on the Engine Thread is required!

```csharp
Core.Environment.InvokeIfRequired(() => {//Do stuff here});
```

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L80-L128

### Video Example
https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/06d27a09-569e-45f8-9cee-aeea5cd3d250

## Exercise 2. Extend Plugin with Internal Communication
* Setting up Internal communication in the plugin requires two methods:

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L63-L78




