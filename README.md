# AspNetWebpack.AssetHelpers

Asset utilities for [AspNetWebpack](https://github.com/Baune8D/AspNetWebpack)

Initialize AssetService in ```Startup.cs```:
```csharp
public IConfiguration Configuration { get; }

public IWebHostEnvironment Env { get; }

public void ConfigureServices(IServiceCollection services)
{
    services.AddAssetHelpers(Configuration, Env);
}
```

Use extensions to get the bundle name:
```csharp
var bundle = ViewData.GetBundleName() // Gets the bundle name from ViewData["Bundle"]
var bundle = Html.GetBundleName() // Gets the bundle name from the view context
```

Recommended use in eg. ```_Layout.cshtml```:
```csharp
var bundle = ViewData.GetBundleName() ?? Html.GetBundleName();
// Gets the bundle name from the view context but allows overriding it in ViewData["Bundle"]
```

Use ```AssetService``` to get assets:
```csharp
@inject IAssetService AssetService

@await AssetService.AssetPath
// Generates: /Path/To/Assets

@await AssetService.GetBundlePathAsync("SomeBundle.js")
// Generates: /Path/To/Assets/SomeBundle.js?v=cache-buster

@await AssetService.GetScriptTagAsync("SomeBundle")
// Generates: <script src="/Path/To/Assets/SomeBundle.js?v=cache-buster"></script>

@await AssetService.GetLinkTagAsync("SomeBundle")
// Generates: <link href="/Path/To/Assets/SomeBundle.css?v=cache-buster" rel=\"stylesheet\" />

@await AssetService.GetStyleTagAsync("SomeBundle")
// Generates: <style>Inlined CSS</style
```
Overloads exists on ```GetScriptTagAsync``` to change the script load behaviour to eg. async.
