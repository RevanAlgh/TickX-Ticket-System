using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;

namespace Ticket.API.Controllers;

[Route("api/[controller]")]
[ApiController]


public class HealthCheckController : ControllerBase
    {
        private readonly IHealthCheck _healthCheckRepository;
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(IHealthCheck healthCheckRepository, ILogger<HealthCheckController> logger)
        {
            _healthCheckRepository = healthCheckRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetHealthStatus(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start GetHealthStatus API");
            var healthCheckContext = new HealthCheckContext(); // You can pass context details if needed
            var result = await _healthCheckRepository.CheckHealthAsync(healthCheckContext, cancellationToken);

            if (result.Status == HealthStatus.Healthy)
            {
                return Ok(new
                {
                    status = "Healthy",
                    description = result.Description
                });
            }

            return StatusCode(503, new
            {
                status = "Unhealthy",
                description = result.Description
            });
        }
    }





