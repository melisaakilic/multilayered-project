namespace Multiple_Layered_Service.Library.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly ILogger<AuthService> _logger;
        readonly ITokenService _tokenService;


        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthService> logger, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<(SignInResult SignInResult, Token? Token)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user is null)
                {
                    _logger.LogWarning("Giriş başarısız: Kullanıcı bulunamadı - {Email}", loginDto.Email);
                    throw new KeyNotFoundException();
                }

                // Kullanıcı kilitli mi kontrol et
                if (await _userManager.IsLockedOutAsync(user))
                {
                    var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                    _logger.LogWarning("Kullanıcı hesabı kilitli: {Email}, Kilit bitiş: {LockoutEnd}", loginDto.Email, lockoutEnd);
                    throw new UnauthorizedAccessException();
                }

                var signInResult = await _signInManager.CheckPasswordSignInAsync(
                    user,
                    loginDto.Password,
                    lockoutOnFailure: true
                );

                if (signInResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.ResetAccessFailedCountAsync(user);
                    _logger.LogInformation("Başarılı giriş: {Email}", loginDto.Email);

                    var token = await _tokenService.CreateToken(user);

                    user.RefreshToken = token.RefreshToken;
                    user.RefreshTokenEndDate = token.RefreshTokenExpiration;
                    await _userManager.UpdateAsync(user);

                    return (signInResult, token);
                }
                else
                {
                    var failCount = await _userManager.GetAccessFailedCountAsync(user);
                    _logger.LogWarning("Başarısız giriş denemesi - {Email}, Deneme sayısı: {FailCount}", loginDto.Email, failCount + 1);

                    // Başarısız giriş sayısını artır
                    await _userManager.AccessFailedAsync(user);
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata: {Email}", loginDto.Email);
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                var userId = _signInManager.Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Çıkış başarısız: Kullanıcı ID'si bulunamadı");
                    throw new UnauthorizedAccessException();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    _logger.LogWarning("Çıkış başarısız: Kullanıcı bulunamadı - ID: {UserId}", userId);
                    throw new KeyNotFoundException();
                }

                user.RefreshToken = null;
                user.RefreshTokenEndDate = DateTime.Now;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignOutAsync();

                _logger.LogInformation("Başarılı Çıkış: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Çıkış işlemi sırasında hata");
                throw;
            }
        }

        public async Task<Response> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                {
                    _logger.LogWarning("Kayıt başarısız: Email zaten kayıtlı - {Email}", registerDto.Email);
                    throw new ArgumentException();
                }

                var user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Kayıt başarısız: {Errors}", result.Errors.First().Description);
                    throw new InvalidOperationException();
                }

                await _userManager.AddToRoleAsync(user, "User");
                var token = await _tokenService.CreateToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.RefreshTokenExpiration;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation("Başarılı kayıt: {Email}", user.Email);

                return new Response
                {
                    Succeeded = true,
                    Message = "Kayıt işlemi başarılı",
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata: {Email}", registerDto.Email);
                throw;
            }
        }
    }
}
