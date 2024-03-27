# Experior Modules: Experior.Core.Plugin

![Plugin](https://github.com/Pasgaard/Xperior-Sandbox/assets/12232128/2ae16ca8-082b-48f9-aeb1-d496fba9096e=20x20)

## Description
_The only difference between an Experior.Core.Plugin and a normal C# Class Library (dynamic link library) is the access to Experior functionality and an embedded ‘tail’ containing a license-key for the plugin to be used in Experior._ 

1. The Plucing basics
2. Extend the Plugin with GUI Buttons
3. 

## The Plugin basics
In the plugin a singleton instance is needed and a Logo to be displayed in the GUI

```csharp
namespace Experior.Plugin.DiscreteEventTraining
{
    public class DiscreteEventTrainingPlugin : Experior.Core.Plugin
    {
        private static DiscreteEventTrainingPlugin _instance; //Singleton

        public DiscreteEventTrainingPlugin()
            : base(nameof(DiscreteEventTrainingPlugin))
        {
            _instance = this;
        }

        public static DiscreteEventTrainingPlugin Instance => _instance ?? (_instance = new DiscreteEventTrainingPlugin());

        public override ImageSource Logo => EmbeddedResource.GetImage("BasicTrainingPlugin");
    }
}
```

##  Extend the Plugin with GUI Buttons

https://github.com/Pasgaard/Xperior-Sandbox/blob/017da174a05fbd07ccac5b7f6e21eb2b3a636403/src/DiscreteEventTrainingPlugin.cs#L22-L49


## Extend Plugin with custom connections


