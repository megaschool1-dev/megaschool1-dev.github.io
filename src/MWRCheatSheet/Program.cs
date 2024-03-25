using Append.Blazor.WebShare;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MWRCheatSheet;
using MWRCheatSheet.Model;
using MWRCheatSheet.Repository;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddScoped(sp => http);

builder.Services.AddSingleton<Repository>();

var clientSettings = await http.GetFromJsonAsync<ClientSettings>("appsettings.json");

builder.Services.AddSingleton(sp =>
{
    return clientSettings?.UI ?? new();
});

builder.Services.AddSingleton(sp =>
{
    return new Constants(sp.GetRequiredService<UISettings>());
});

builder.Services.AddMudServices();
builder.Services.AddWebShare();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
