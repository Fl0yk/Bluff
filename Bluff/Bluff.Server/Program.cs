using Bluff.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<GameHub>("/game");

app.Run();
