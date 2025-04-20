using Authentication_Service.Models;
using Authentication_Service.Repositories;
using Authentication_Service.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication_Service.Services;

public class AuthService : IAuthService
{
    private readonly IStaffRepository _repository;
    private readonly IConfiguration _config;

    public AuthService(IStaffRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(Staff staff)
    {
        var error = Validaciones.ValidarStaff(staff);
        if (error != null)
            return (false, error);

        if (await _repository.GetByCiAsync(staff.Ci) is not null)
            return (false, "El CI ya está registrado.");

        staff.Id = Guid.NewGuid();
        staff.Password = BCrypt.Net.BCrypt.HashPassword(staff.Password);
        await _repository.AddAsync(staff);
        return (true, "Registrado con éxito");
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetByCiAsync(request.Ci);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(Staff user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("ci", user.Ci.ToString()),
            new Claim("numero", user.NumerPhone.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "auth_service",
            audience: "auth_service_users",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
