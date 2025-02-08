# SceneFlow
Easy way to implement a loading screen.

## Authors

- [@Kévin "Kebab" Reilhac](https://www.github.com/KevinReilhac)


## Installation

Install SceneFlow with Unity Package Manager

```bash
  https://github.com/KevinReilhac/SceneFlow.git#upm
```

## Initalization

- Setup your SceneFlowSettings in project settings.

![image](https://github.com/user-attachments/assets/5395564c-41c1-49d2-9116-93268ab5f9ad)

- For fast setup, you can install and modify a LoadingScreen from the package samples.
 
![image](https://github.com/user-attachments/assets/e991af1f-0ca1-4780-b2b2-0a77d79785d7)


- or you can also create a custom Loading screen prefab by creating a script that inherite from `ALoadingScreen`.
```csharp
public class CustomLoadingScreen : ALoadingScreen
{
    [SerializeField] private Image progressBar;

    public override void Hide() => gameObject.SetActive(false);
    public override void Show() => gameObject.SetActive(true);

    public override void UpdateProgress(float progress)
    {
        base.UpdateProgress(progress);
        progressBar.fillAmount = progress;
    }
}
```

- Then set the prefab as used loading screen prefab by clicking on `Set as loading screen`

![image](https://github.com/user-attachments/assets/cdcbba0a-6225-489a-a948-46f98eb2f3bf)

## Load a scene
```csharp
SceneFlowManager.Load("MyScene"); //Load a scene by name
SceneFlowManager.Load(1, showLoadingScreen: false); //Load a scene by build index
SceneFlowManager.LoadNextScene(); //Load the next scene in the build settings
```
showLoadingScreen parameter is optional, default is true.

## Components
### LoadSceneAfterTime
Load a scene after a delay.

![image](https://github.com/user-attachments/assets/1141938d-2c05-4d51-bc11-858079122945)
__________
### LoadSceneOnEvent
Load a scene when an external UnityEvent call LoadScene method.

![image](https://github.com/user-attachments/assets/18267907-b22d-41de-9bfb-5b114da7e6e3)
__________
### LoadNextSceneOnEvent
Load the next scene when an event is triggered.

![image](https://github.com/user-attachments/assets/c61a6629-a822-46d9-aea4-dc48f35571bf)
## Documentation

[Read Documentation](https://kevinreilhac.github.io/SceneFlow/)
