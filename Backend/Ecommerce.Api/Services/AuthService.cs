using Microsoft.AspNetCore.Identity;
using Ecommerce.Api.DTOs;
using Ecommerce.Api.Models;

namespace Ecommerce.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<ResultadoOperacion<AuthResponseDto>> RegistrarAsync(RegistroDto dto)
    {
        var usuarioExistente = await _userManager.FindByEmailAsync(dto.Email);
        if (usuarioExistente is not null)
        {
            return ResultadoOperacion<AuthResponseDto>.Fallo(
                "Ya existe una cuenta registrada con ese email", TipoError.ValidacionNegocio);
        }

        var nuevoUsuario = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido
        };


        var resultado = await _userManager.CreateAsync(nuevoUsuario, dto.Password);

        if (!resultado.Succeeded)
        {
            var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
            return ResultadoOperacion<AuthResponseDto>.Fallo(errores, TipoError.ValidacionNegocio);
        }


        await _userManager.AddToRoleAsync(nuevoUsuario, "Cliente");

        var respuesta = GenerarRespuesta(nuevoUsuario, ["Cliente"]);
        return ResultadoOperacion<AuthResponseDto>.Ok(respuesta);
    }

    public async Task<ResultadoOperacion<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var usuario = await _userManager.FindByEmailAsync(dto.Email);
        if (usuario is null)
        {
            return ResultadoOperacion<AuthResponseDto>.Fallo(
                "Email o contraseña incorrectos", TipoError.ValidacionNegocio);
        }

        var passwordValida = await _userManager.CheckPasswordAsync(usuario, dto.Password);
        if (!passwordValida)
        {
            return ResultadoOperacion<AuthResponseDto>.Fallo(
                "Email o contraseña incorrectos", TipoError.ValidacionNegocio);
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        var respuesta = GenerarRespuesta(usuario, roles);
        return ResultadoOperacion<AuthResponseDto>.Ok(respuesta);
    }

    private AuthResponseDto GenerarRespuesta(ApplicationUser usuario, IList<string> roles)
    {
        var (token, expiracion) = _tokenService.GenerarToken(usuario, roles);

        return new AuthResponseDto
        {
            Token = token,
            Expiracion = expiracion,
            Nombre = $"{usuario.Nombre} {usuario.Apellido}",
            Email = usuario.Email!,
            Roles = roles.ToList()
        };
    }
}