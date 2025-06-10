using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD_Task10.App.Helpers.Options;
using APBD_Task10.App.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Task10.App;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    
    public string GenerateToken(string username, string role)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim("role", role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.ValidInMinutes)),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}