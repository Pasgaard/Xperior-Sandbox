# Experior Modules: Experior.Core.Plugin

![Plugin](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/2ae16ca8-082b-48f9-aeb1-d496fba9096e=20x20)

## Description
_The only difference between an Experior.Core.Plugin and a normal C# Class Library (dynamic link library) is the access to Experior functionality and an embedded ‘tail’ containing a license-key for the plugin to be used in Experior._ 

1. The Plucing basics
2. Extend the Plugin with GUI Buttons
3. 

## The Plugin basics
The class **DiscreteEventTrainingPlugin** will be instantiated by Experior during starting process.
Here an example of the default content from the template.
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



![image](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/f08c6075-2635-41c8-a559-99ea54713277)

##  Extend the Plugin with GUI Buttons

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L22-L49


## Extend Plugin with custom connections


