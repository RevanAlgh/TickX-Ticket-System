using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ticket.Data.Models.Data;


namespace Ticket.Service.HealthChecks;

public class HealthCheckRepository : IHealthCheck
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HealthCheckRepository> _logger;
    private readonly IConfiguration _configuration;

    public HealthCheckRepository(ApplicationDbContext context, HttpClient httpClient, ILogger<HealthCheckRepository> logger, IConfiguration configuration)
    {
        _context = context;
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var healthCheckResults = new List<string>();
        var isHealthy = true;

        // External API health check (ChatGPT)
        if (!await CheckExternalApiHealthAsync(cancellationToken))
        {
            isHealthy = false;
            healthCheckResults.Add("External API (ChatGPT) is not reachable.");
        }

        // Check if Users table is not empty
        if (!CheckUsersTableNotEmpty())
        {
            isHealthy = false;
            healthCheckResults.Add("Users table is empty.");
        }

        // Check if any user has an unhashed password
        if (CheckForUnhashedPasswords())
        {
            isHealthy = false;
            healthCheckResults.Add("One or more users have an unhashed password.");
        }

        // Disk space health check
        if (!CheckDiskSpaceHealth())
        {
            isHealthy = false;
            healthCheckResults.Add("Insufficient disk space available.");
        }

        // Memory usage health check
        if (!CheckMemoryHealth())
        {
            isHealthy = false;
            healthCheckResults.Add("Memory usage is too high.");
        }

        // Combine all results into one message
        var resultDescription = string.Join(" + ", healthCheckResults);

        // If all checks pass, return Healthy
        return isHealthy
                   ? HealthCheckResult.Healthy("All health checks passed.")
                   : HealthCheckResult.Unhealthy(resultDescription);
    }

    private async Task<bool> CheckExternalApiHealthAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve API key from configuration
            var apiKey = _configuration["OpenAI:ApiKey"];
            var baseUrl = _configuration["OpenAI:TheBaseurl"];

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("ChatGPT API key is missing.");
                return false;
            }

            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.LogError("ChatGPT API BaseUrl is missing.");
                return false;
            }

            // Prepare the request
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "user", content = "health check" } },
                max_tokens = 1 // Minimal token usage to reduce impact
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync(baseUrl, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"ChatGPT API health check failed: {response.StatusCode} - {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"External API health check failed: {ex.Message}");
            return false;
        }
    }


    private bool CheckUsersTableNotEmpty()
    {
        try
        {
            return _context.Users.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Users table check failed: {ex.Message}");
            return false;
        }
    }

    private bool CheckForUnhashedPasswords()
    {
        try
        {
            // Check if there are any users with a password that doesn't start with "$2a$", "$2b$", or "$2y$"
            return _context.Users.Any(u =>
                !(u.Password.StartsWith("$2a$") ||
                  u.Password.StartsWith("$2b$") ||
                  u.Password.StartsWith("$2y$")));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Check for unhashed passwords failed: {ex.Message}");
            return false;
        }
    }

    private bool CheckDiskSpaceHealth()
    {
        var driveInfo = new DriveInfo("C");
        return driveInfo.AvailableFreeSpace > 1_000_000_000; // Check if there is more than 1GB free
    }

    private bool CheckMemoryHealth()
    {
        var usedMemory = GC.GetTotalMemory(false);
        return usedMemory < 1_000_000_000; // Example: check if memory usage is under 1GB
    }
}




