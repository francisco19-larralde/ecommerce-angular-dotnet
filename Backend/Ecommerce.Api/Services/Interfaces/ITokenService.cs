using Ecommerce.Api.Models;

namespace Ecommerce.Api.Services;

public interface ITokenService
{
    (string Token, DateTime Expiracion) GenerarToken(ApplicationUser usuario, IList<string> roles);
}