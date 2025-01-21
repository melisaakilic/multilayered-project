namespace Multiple_Layered.API.Extensions.Middlewares
{
    public class ExceptionMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata meydana geldi");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorDetails = new ErrorDetails();

            switch (exception)
            {
                case UnauthorizedAccessException:
                    errorDetails.StatusCode = StatusCodes.Status401Unauthorized;
                    errorDetails.Message = "Yetkisiz erişim";
                    break;

                case KeyNotFoundException:
                    errorDetails.StatusCode = StatusCodes.Status404NotFound;
                    errorDetails.Message = "Kayıt bulunamadı";
                    break;

                case ValidationException validationException:
                    errorDetails.StatusCode = StatusCodes.Status400BadRequest;
                    errorDetails.Message = "Doğrulama hatası";
                    errorDetails.Details = validationException.Message;
                    break;

                case ArgumentException argException:
                    errorDetails.StatusCode = StatusCodes.Status400BadRequest;
                    errorDetails.Message = "Geçersiz argüman";
                    errorDetails.Details = argException.Message;
                    break;

                case InvalidOperationException:
                    errorDetails.StatusCode = StatusCodes.Status400BadRequest;
                    errorDetails.Message = "Geçersiz işlem";
                    break;

                case DbUpdateException dbUpdateEx:
                    errorDetails.StatusCode = StatusCodes.Status409Conflict;
                    errorDetails.Message = "Veritabanı güncelleme hatası";
                    errorDetails.Details = dbUpdateEx.InnerException?.Message;
                    break;

                case NotImplementedException:
                    errorDetails.StatusCode = StatusCodes.Status501NotImplemented;
                    errorDetails.Message = "Bu özellik henüz implementte edilmemiş";
                    break;

                case TimeoutException:
                    errorDetails.StatusCode = StatusCodes.Status504GatewayTimeout;
                    errorDetails.Message = "İşlem zaman aşımına uğradı";
                    break;

                case BadHttpRequestException badRequestEx:
                    errorDetails.StatusCode = StatusCodes.Status400BadRequest;
                    errorDetails.Message = "Geçersiz HTTP isteği";
                    errorDetails.Details = badRequestEx.Message;
                    break;

                case OperationCanceledException:
                    errorDetails.StatusCode = StatusCodes.Status408RequestTimeout;
                    errorDetails.Message = "İşlem iptal edildi";
                    break;

                case IOException:
                    errorDetails.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    errorDetails.Message = "I/O işlemi başarısız";
                    break;

                default:
                    errorDetails.StatusCode = StatusCodes.Status500InternalServerError;
                    errorDetails.Message = "Sunucuda beklenmeyen bir hata meydana geldi";
                    break;
            }

            context.Response.StatusCode = errorDetails.StatusCode;
            await context.Response.WriteAsync(errorDetails.ToString());
        }

    }
}
