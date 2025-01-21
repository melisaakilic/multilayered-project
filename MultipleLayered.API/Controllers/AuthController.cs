using Microsoft.AspNetCore.Authorization;

namespace Multiple_Layered.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var (signInResult, token) = await _authService.LoginAsync(loginDto);
            _logger.LogInformation("Kullanıcı Giriş Denemesi: {Email}", loginDto.Email);

            if (signInResult.IsLockedOut)
                return BadRequest("Çok fazla başarısız deneme yaptınız. Daha sonra tekrar deneyin!");

            if (!signInResult.Succeeded)
                return BadRequest("Lütfen Bilgilerinizi Tekrar Kontrol Edin!");

            return Ok(new
            {
                Succeeded = true,
                Message = "Giriş başarılı",
                Token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Giriş Sırasında Bir Hata Meydana Geldi: {Email}", loginDto.Email);
            throw;
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            _logger.LogInformation("Kayıt Denemesi: {Email}", registerDto.Email);
            return result.Succeeded ? Ok() : BadRequest("Kayıt İşlemi Başarısız Oldu.!!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kayıt Sırasında Hata Meydana Geldi: {Email}", registerDto.Email);
            throw;
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("Kullanıcı Çıkış Yaptı");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Çıkış Yapılırken Hata Meydana Geldi");
            throw;
        }
    }
}