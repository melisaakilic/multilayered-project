namespace Multiple_Layered_Service.Library.Services.JwtServices
{
    public class TokenService : ITokenService
    {
        readonly CustomTokenOptions _customTokenOptions;
        readonly UserManager<User> _userManager;

        public TokenService(CustomTokenOptions customTokenOptions, UserManager<User> userManager)
        {
            _customTokenOptions = customTokenOptions;
            _userManager = userManager;
        }

        public async Task <Token> CreateToken(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_customTokenOptions.SecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: await GetClaims(user, _customTokenOptions.Audience),
                signingCredentials: signingCredentials
            );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            return new Token
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }

        private string CreateRefreshToken()
        {
            var numberBytes = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(numberBytes);
            return Convert.ToBase64String(numberBytes);
        }

        private async Task <IEnumerable<Claim>> GetClaims(User user, List<string> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            userList.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            userList.AddRange(audiences.Select(audience => new Claim(JwtRegisteredClaimNames.Aud, audience)));
            return userList;
        }
    }
}
