using Blazored.Modal;
using Bluff.Client;
using Bluff.Client.models;
using Bluff.Client.Services.InGame;
using Bluff.Client.Services.Servers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IInGameHubService, InGameHubService>();
builder.Services.AddScoped<IServersHubService, ServersHubService>();
builder.Services.AddScoped<GameModel>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredModal();

await builder.Build().RunAsync();
