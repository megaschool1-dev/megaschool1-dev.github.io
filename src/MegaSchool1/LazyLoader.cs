using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace  MegaSchool1;

public class LazyLoader
{
    public List<Assembly> AdditionalAssemblies { get; } = new();

    private readonly LazyAssemblyLoader _lazyAssemblyLoader;

    private readonly NavigationManager _navigationManager;

    private readonly ILogger<LazyLoader> _logger;

    public LazyLoader(
        LazyAssemblyLoader lazyAssemblyLoader,
        NavigationManager navigationManager,
        ILogger<LazyLoader> logger)
    {
        this._lazyAssemblyLoader = lazyAssemblyLoader;
        this._navigationManager = navigationManager;
        this._logger = logger;
    }

    public async Task OnNavigateAsync(NavigationContext context) =>
        await this.OnNavigateAsync(context.Path.Trim('/'));

    public async Task PreloadAsync()
    {
        var uri = new Uri(this._navigationManager.Uri);
        await this.OnNavigateAsync(uri.LocalPath.Trim('/'));
    }

    public async Task OnNavigateAsync(string path)
    {
        try
        {
            // 👇 Load lazy assemblies that are needed for the current URL path. 
            if (path == "shareables")
            {
                var assemblies = await this._lazyAssemblyLoader
                    .LoadAssembliesAsync([]);
                this.AdditionalAssemblies.AddRange(assemblies);
            }
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Error loading assemblies");
        }
    }
}