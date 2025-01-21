namespace Multiple_Layered_Service.Library.Services.JwtServices
{
    public interface ITokenService
    {
        Task <Token> CreateToken(User user);
    }
}
