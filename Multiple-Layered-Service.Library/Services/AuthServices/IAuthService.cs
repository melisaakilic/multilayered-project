namespace Multiple_Layered_Service.Library.Services.AuthRepo
{
    public interface IAuthService
    {
        Task<(SignInResult SignInResult, Token? Token)> LoginAsync(LoginDto loginDto);
        Task<Response> RegisterAsync(RegisterDto registerDto);
        Task LogoutAsync();
    }
}
