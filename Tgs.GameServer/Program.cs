using System.Text.Json.Serialization;
using FluentdForward.OpenTelemetry.MessagePack;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
	.AddOpenTelemetry(options =>
	{
		options.IncludeFormattedMessage = true;
		options.ParseStateValues = true;
		_ = options.AddFluentdForwardExporter(fluentOpt =>
		{
			var nodeIp = builder.Configuration.GetValue<string>("NODE_IP")!;

			fluentOpt.Host = nodeIp;
			fluentOpt.Tag = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME")!;
			fluentOpt.UseMessagePack(LogRecordFormatterResolver.GetResolverInstanceWithExtendInfo(
				"MachineName",
				() => new
				{
					Environment.MachineName
				}));
		});
	});

builder.Services
	.AddRouting(routeOpt => routeOpt.LowercaseUrls = true)
	.AddControllers()
	.AddJsonOptions(options => options
		.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
;

builder.Services
	.AddGameServer()
	.AddGameServerRepositories((_, dbBuilder) => dbBuilder
		.UseNpgsql("Name=Tgs"));

builder.Services
	.AddSwaggerGen(c =>
	{
		var xmlFileNames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml");
		foreach (var xmlFileName in xmlFileNames)
			c.IncludeXmlComments(xmlFileName);
	});

builder.Services
	.AddHealthChecks();

builder.Services
	.AddOpenTelemetry()
	.WithTracing(tracingBuilder => tracingBuilder
		.SetResourceBuilder(ResourceBuilder.CreateDefault()
			.AddEnvironmentVariableDetector())
		.AddSource("Tgs.*")
		.AddSource("TGS.*")
		.AddNpgsql()
		.AddAspNetCoreInstrumentation()
		.AddProfileViewExporter()
		.AddOtlpExporter())
	.WithMetrics(meterBuiler => meterBuiler
		.SetResourceBuilder(ResourceBuilder.CreateDefault()
			.AddEnvironmentVariableDetector())
			.AddRuntimeInstrumentation()
			.AddAspNetCoreInstrumentation()
			.AddOtlpExporter())
	.Services
	.AddProfileViewer()
	.Configure<AspNetCoreInstrumentationOptions>(options => options.Filter =
		httpContext => !httpContext.Request.Path.HasValue
			|| !httpContext.Request.Path.StartsWithSegments("/swagger")
			&& !httpContext.Request.Path.StartsWithSegments("/healthz")
			&& !httpContext.Request.Path.StartsWithSegments("/metrics")
			&& !httpContext.Request.Path.StartsWithSegments("/profiler/")
			&& !new[] { "html", "ico", "png", "jpg" }.Any(
				ext => httpContext.Request.Path.Value.EndsWith($".{ext}")));

var app = builder.Build();

app.MapControllers();
app.MapSwagger();
app.MapProfileViewer();
app.MapHealthChecks("/healthz");

app.UseSwaggerUI();

app.Run();
