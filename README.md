# WOW.DI

WOW.DI is a unity package/library for Dependency Injection.　It is inspired by Vue.js.

## Getting Started

### Provide

Create a provider class that inherits from `Provider` and attach it to a GameObject. The scope of Provider matches the parent-child relationship of GameObject.

Override the `Install()` to define how instances are to be created.

```c#
// Define interfaces for the class to provide 
public interface IWeatherService
{
    Task<Weather> GetWeatherAsync(string id);
}

public class WeatherServiceJMA : IWeatherService
{
    public Task<Weather> GetWeatherAsync(string id)
    {
        // get weather information from JMA.
    }
}

public class WeatherServiceMock : IWeatherService
{
    public Task<Weather> GetWeatherAsync(string id)
    {
        // returns mock values.
        return new Weather();
    }
}
```

```c#
using WOW.DI;

public class ProviderWeather : Provider
{
    public override void Install()
    {
        // Single instance can be registered. 
        this.Provide<IWeatherService>().AsSingle<WeatherServiceJMA>();

        // Multiple instances can be registered by the key.
        this.Provide<IWeatherService>().WithKey<WeatherServiceMock>("Mock");
    }
}
```

#### ProviderApp

`ProviderApp` is always created immediately before the scene loaded. When customizing the `Install()`, create ProviderApp Prefab on `/Resources` and attach a class that inherits `ProviderAppInstallerBase` to it.

```c#
public class WeatherServiceInstaller : ProviderAppInstallerBase
{
    public override void InstallTo(ProviderApp provider)
    {
        provider.Provide<IWeatherService>().AsSingle<WeatherServiceJMA>();
    }
}
```

### Injection

Use extension methods of Behaviour to inject at any time.

```c#
// Attached in the child of WeatherProvider.
public class WeatherForecast : MonoBehaviour
{
    private async void Start()
    {
        // Get WeatherServiceJMA
        var weatherService = this.Inject<IWeatherService>();
        var response = await weatherService.GetWeatherAsync("0000");
    }
}
```

Attach the `InjectAttribute` to fields, it will be automatically injected on `Awake()`.

```c#
public class WeatherForecast : MonoBehaviour
{
    // Auto Injection
    [Inject]
    private IWeatherService weatherService = null;

    private async void Start()
    {
        var response = await weatherService.GetWeatherAsync("0000");
    }
}
```

When dynamically instantiate Prefabs, it will be automatically injected by using `InjectInstantiate`.

```c#
public class ExamplePrefab : MonoBehaviour
{
    [Inject]
    private IWeatherService weatherService = null;
}

public class WeatherForecast : MonoBehaviour
{
    [SerializeField]
    private ExamplePrefab prefab;

    private void Start()
    {
        var obj = this.InjectInstantiate(prefab, this.transform);
    }
}
```

## License

The license is MIT license.

## Contributers

- [umetaman](https://github.com/umetaman/)