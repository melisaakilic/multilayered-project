namespace Multiple_Layered.API.Extensions.Exceptions
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime TimesStamp { get; set; } = DateTime.Now;


        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
