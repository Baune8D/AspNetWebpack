# AspNetWebpack
[![Build status](https://ci.appveyor.com/api/projects/status/u369u4wt45hsw53f?svg=true)](https://ci.appveyor.com/project/Baune8D/aspnetwebpack)
[![codecov](https://codecov.io/gh/Baune8D/AspNetWebpack/branch/main/graph/badge.svg?token=M4KiXgJBnw)](https://codecov.io/gh/Baune8D/AspNetWebpack)
[![NuGet Badge](https://buildstats.info/nuget/AspNetWebpack)](https://www.nuget.org/packages/AspNetWebpack)

Asset utilities for [AspNetWebpack](https://github.com/Baune8D/AspNetWebpack)

Initialize AssetService in ```Startup.cs```:
```csharp
public IConfiguration Configuration { get; }

public IWebHostEnvironment Env { get; }

public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddAspNetWebpack(Configuration, Env);
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
Overloads exists on ```GetBundlePathAsync``` in case no extension is applied to the bundle name.

Overloads exists on ```GetScriptTagAsync``` to change the load behaviour to eg. ```async``` and/or ```defer```.

A fallback bundle can be set on: ```GetScriptTagAsync```, ```GetLinkTagAsync```, ```GetStyleTagAsync```
```csharp
@await AssetService.GetScriptTagAsync("SomeBundle", "FallbackBundle")
// Generates: <script src="/Path/To/Assets/SomeBundle.js?v=cache-buster"></script>
// Or if SomeBundle does not exist: <script src="/Path/To/Assets/FallbackBundle.js?v=cache-buster"></script>
```

## Example _Layout.cshtml

```razor
@using AspNetWebpack

@inject IAssetService AssetService

@{
    var bundle = ViewData.GetBundleName() ?? Html.GetBundleName();
}

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - AspNetWebpack</title>
    @await RenderSectionAsync("Head", required: false)

    @await AssetService.GetLinkTagAsync(bundle, "Layout");
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    @RenderBody()

    @await AssetService.GetScriptTagAsync(bundle, "Layout", ScriptLoad.Defer);
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```
