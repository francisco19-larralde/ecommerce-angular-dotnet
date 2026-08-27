using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public interface IAuthService
{
    Task<ResultadoOperacion<AuthResponseDto>> RegistrarAsync(RegistroDto dto);
    Task<ResultadoOperacion<AuthResponseDto>> LoginAsync(LoginDto dto);
}