using Bluff.Server.Hubs;
using Bluff.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddSingleton<IGroupService, GroupService>();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ServerHub>("/servers");
app.MapHub<InGameHub>("/game");

app.Run();
