using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ecommerce.Api.Models;

namespace Ecommerce.Api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime Expiracion) GenerarToken(ApplicationUser usuario, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id),
            new(JwtRegisteredClaimNames.Email, usuario.Email!),
            new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}")
        };


        claims.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var minutos = int.Parse(_configuration["Jwt:ExpiracionMinutos"]!);
        var expiracion = DateTime.UtcNow.AddMinutes(minutos);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiracion);
    }
}