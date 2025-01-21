namespace Multiple_Layered_Service.Library.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        readonly UserManager<User> _userManager;
        readonly RoleManager<Role> _roleManager;
        readonly ILogger<RoleService> _logger;

        public RoleService(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<RoleService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<Response> AssignRoleAsync(AssignRoleDto request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user is null)
                {
                    _logger.LogWarning("Kullanıcı bulunamadı: {UserId}", request.UserId);
                    throw new ArgumentException();
                }

                if (!await _roleManager.RoleExistsAsync(request.Role))
                {
                    _logger.LogWarning("Geçersiz rol: {Role}", request.Role);
                    throw new ArgumentException();
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Contains(request.Role))
                {
                    _logger.LogWarning("Kullanıcı zaten bu role sahip: {Role}", request.Role);
                    throw new InvalidOperationException();
                }

                var result = await _userManager.AddToRoleAsync(user, request.Role);
                if (!result.Succeeded)
                {
                    _logger.LogError("Rol atama başarısız: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new InvalidOperationException();
                }

                _logger.LogInformation("Rol başarıyla atandı. Kullanıcı: {UserId}, Rol: {Role}",request.UserId, request.Role);

                return new Response
                {
                    Succeeded = true,
                    Message = "Rol başarıyla atandı"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol atama işleminde hata: {UserId}, {Role}", request.UserId, request.Role);
                throw;
            }
        }
    }
}
