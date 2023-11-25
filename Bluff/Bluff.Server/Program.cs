using Bluff.Server.Hubs;
using Bluff.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddSingleton<IGroupService, GroupService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ServerHub>("/game");

app.Run();
