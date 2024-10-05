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

var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddScoped(sp => http);

builder.Services.AddScoped<Repository>(sp => new(sp.GetRequiredService<ILocalStorageService>(), sp.GetRequiredService<HttpClient>()));

var clientSettings = await http.GetFromJsonAsync<ClientSettings>("appsettings.json", new System.Text.Json.JsonSerializerOptions() { Converters = { new UnknownEnumConverter() } });

builder.Services
    .AddSingleton(sp => clientSettings?.UI ?? new())
    .AddSingleton(sp => new Constants(sp.GetRequiredService<UISettings>(), sp.GetRequiredService<NavigationManager>()))
    .AddSingleton(sp => new Mappers());

builder.Services.AddMudServices();
builder.Services.AddWebShare();
builder.Services.AddBlazoredLocalStorage();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Memory()
    .WriteTo.BrowserConsole()
    .CreateLogger();

await builder.Build().RunAsync();
