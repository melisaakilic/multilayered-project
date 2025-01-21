namespace Multiple_Layered_Service.Library.Dtos.AuthDtos
{
    public class Response
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public Token? Token { get; set; }
    }
}
