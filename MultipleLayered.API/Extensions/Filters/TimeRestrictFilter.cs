using Microsoft.AspNetCore.Mvc.Filters;

public class TimeRestrictAttribute : ActionFilterAttribute
{
    private readonly TimeOnly _startTime;
    private readonly TimeOnly _endTime;
    

    public TimeRestrictAttribute(string startTime = "06:00", string endTime = "23:59")
    {
        _startTime = TimeOnly.Parse(startTime);
        _endTime = TimeOnly.Parse(endTime);

    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        
        var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
        loggerFactory.CreateLogger<TimeRestrictAttribute>();

        var currentTime = TimeOnly.FromDateTime(DateTime.Now);

        if (currentTime < _startTime || currentTime > _endTime)
        {
            

            context.Result = new JsonResult(new
            {
                Error = "Bu API'ye sadece mesai saatleri içinde erişilebilir, eğer bu ayarı kapatmak istiyorsanız controller'dan attribute'u kaldırın.",
                WorkingHours = $"{_startTime} - {_endTime}",
                CurrentTime = currentTime.ToString()
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        
        base.OnActionExecuting(context);
    }
}