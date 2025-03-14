using Append.Blazor.WebShare;
using Blazored.LocalStorage;
using MegaSchool1.Model;
using MegaSchool1.Repository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http.Json;
using MegaSchool1;
using Microsoft.AspNetCore.Components;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress, builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Memory()
    .WriteTo.BrowserConsole()
    .CreateLogger();

await builder.Build().RunAsync();
static void ConfigureServices(IServiceCollection services, string baseAddress,  IConfiguration configuration)
{
    var http = new HttpClient { BaseAddress = new Uri(baseAddress) };
    services.AddScoped(sp => http);

    services.AddScoped<Repository>(sp => new(sp.GetRequiredService<ILocalStorageService>(), sp.GetRequiredService<HttpClient>()));
    
    var clientSettings = configuration.Get<ClientSettings>();
    
    services
        .AddSingleton(clientSettings!)
        .AddSingleton(sp => clientSettings?.UI ?? new())
        .AddSingleton(sp => new Constants(sp.GetRequiredService<UISettings>(), sp.GetRequiredService<NavigationManager>()))
        .AddSingleton(sp => new Mappers());

    services.AddMudServices();
    services.AddWebShare();
    services.AddBlazoredLocalStorage();
}