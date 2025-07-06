using System.Text.Json;

public class GlobalExceptionCatcher
{
    private readonly RequestDelegate _pipeline;
    private readonly ILogger<GlobalExceptionCatcher> _log;

    public GlobalExceptionCatcher(RequestDelegate pipeline, ILogger<GlobalExceptionCatcher> log)
    {
        _pipeline = pipeline;
        _log = log;
    }

    public async Task Invoke(HttpContext http)
    {
        try
        {
            await _pipeline(http);
        }
        catch (Exception error)
        {
            _log.LogError(error, "An unexpected error occurred.");

            http.Response.StatusCode = StatusCodes.Status500InternalServerError;
            http.Response.ContentType = "application/json";

            var payload = JsonSerializer.Serialize(new { message = "An error occurred while processing your request." });
            await http.Response.WriteAsync(payload);
        }
    }
}

public class RequestResponseLogger
{
    private readonly RequestDelegate _pipeline;
    private readonly ILogger<RequestResponseLogger> _log;

    public RequestResponseLogger(RequestDelegate pipeline, ILogger<RequestResponseLogger> log)
    {
        _pipeline = pipeline;
        _log = log;
    }

    public async Task Invoke(HttpContext http)
    {
        var method = http.Request.Method;
        var endpoint = http.Request.Path;

        _log.LogInformation("Incoming => {Method} {Endpoint}", method, endpoint);

        await _pipeline(http);

        _log.LogInformation("Outgoing => Status Code: {Code}", http.Response.StatusCode);
    }
}

public class SimpleTokenValidator
{
    private readonly RequestDelegate _pipeline;

    public SimpleTokenValidator(RequestDelegate pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task Invoke(HttpContext http)
    {
        var authHeader = http.Request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.Split(" ").Last();

        const string VALID_TOKEN = "mySimpleToken";

        if (token != VALID_TOKEN)
        {
            http.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await http.Response.WriteAsync("Access denied. Invalid token.");
            return;
        }

        await _pipeline(http);
    }
}
