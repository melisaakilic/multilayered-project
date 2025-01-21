namespace Multiple_Layered.API.Extensions.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var startTime = DateTime.UtcNow;

                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                var requestInfo = new
                {
                    Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                    Method = context.Request.Method,
                    ClientIp = clientIp,
                    UserId = userId,
                    StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
                };

                _logger.LogInformation("Request başladı: {@RequestInfo}", requestInfo);

                await _next(context);

                var duration = DateTime.UtcNow - startTime;

                var responseInfo = new
                {
                    RequestInfo = requestInfo,
                    StatusCode = context.Response.StatusCode,
                    Duration = duration.TotalMilliseconds
                };

                _logger.LogInformation("Request tamamlandı: {@ResponseInfo}", responseInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request işlenirken hata oluştu");
                throw;
            }
        }
    }
}
