using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
	.AddOpenTelemetry(options =>
	{
		options.IncludeFormattedMessage = true;
		options.ParseStateValues = true;
		options.AddOtlpExporter();
	});

builder.Services
	.AddControllers();

builder.Services
	.AddGameServer()
	.AddGameServerRepositories((_, dbBuilder) => dbBuilder
		.UseNpgsql("Name=Tgs"));

builder.Services
	.AddHealthChecks();

var app = builder.Build();

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();
