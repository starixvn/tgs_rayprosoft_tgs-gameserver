using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers();

builder.Services
	.AddGameServer()
	.AddGameServerRepositories((_, dbBuilder) => dbBuilder
		.UseNpgsql("Name=Tgs"));

var app = builder.Build();

app.MapControllers();

app.Run();
