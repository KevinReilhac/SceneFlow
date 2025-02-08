# SceneFlow
Easy way to implement a loading screen.

## Authors

- [@Kévin "Kebab" Reilhac](https://www.github.com/KevinReilhac)


## Installation

Install SceneFlow with Unity Package Manager

```bash
  https://github.com/KevinReilhac/SceneFlow.git#upm
```

## Usage/Examples

### Initalization

Setup your SceneFlowSettings in project settings.

For fast setup, you can install and modify a LoadingScreen from the package samples.

or you can also create a custom Loading screen prefab by inheriting from ALoadingScreen.


### Load a scene
```csharp
SceneFlowManager.Load("MyScene"); //Load a scene by name
SceneFlowManager.Load(1, showLoadingScreen: false); //Load a scene by build index
SceneFlowManager.LoadNextScene(); //Load the next scene in the build settings
```
showLoadingScreen parameter is optional, default is true.

### Components

#### LoadSceneAfterTime
Load a scene after a delay.

#### LoadSceneOnEvent
Load a scene when an external UnityEvent call LoadScene method.

#### LoadNextSceneOnEvent
Load the next scene when an event is triggered.

## Documentation

[Read Documentation](https://kevinreilhac.github.io/SceneFlow/)