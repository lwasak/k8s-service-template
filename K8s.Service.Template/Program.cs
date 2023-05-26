using K8s.Service.Template.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("APP_PREFIX:");

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

builder.Services
    .AddHealthChecks()
    .AddCheck<ReadinessHealthCheck>("Readiness", HealthStatus.Degraded, new[] { "readiness" })
    .AddCheck<LivenessHealthCheck>("Liveness", HealthStatus.Degraded, new[] { "liveness" })
    .ForwardToPrometheus();

var app = builder.Build();

app.UseMetricServer();
app.UseRouting();
// IMPORTANT: UseHttpMetrics must be after UseRouting
app.UseHttpMetrics();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Tags.Contains("readiness")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Tags.Contains("liveness")
});

try
{
    await app.RunAsync();
}
finally
{
    await Log.CloseAndFlushAsync();
}
