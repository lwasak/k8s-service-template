using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace K8s.Service.Template.HealthChecks;

public class ReadinessHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) =>
        Task.FromResult(HealthCheckResult.Healthy());
}