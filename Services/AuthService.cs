using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication_Service.Models;
using Authentication_Service.Repositories;
using Authentication_Service.Utils;
using Microsoft.IdentityModel.Tokens;

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
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

        var properties = typeof(Staff)
            .GetProperties()
            .Where(prop => prop.Name != "Password")
            .ToList();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(user)?.ToString();
            if (value != null)
            {
                claims.Add(new Claim(prop.Name, value));
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "auth_service",
            audience: "auth_service_users",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(bool Success, string Message)> ChangePasswordAsync(
        ChangePasswordRequest request
    )
    {
        var user = await _repository.GetByCiAsync(request.Ci);
        if (user == null)
            return (false, "Usuario no encontrado");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password))
            return (false, "La contraseña actual no es correcta");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _repository.UpdateAsync(user);

        return (true, "Contraseña actualizada con éxito");
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(
        ResetPasswordRequest request
    )
    {
        var user = await _repository.GetByCiAsync(request.Ci);
        if (user == null || user.NumberPhone != request.NumberPhone)
            return (false, "CI o número de teléfono incorrectos");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _repository.UpdateAsync(user);

        return (true, "Contraseña restablecida con éxito");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(
        Guid userId,
        UpdateStaffRequest request
    )
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null)
            return (false, "Usuario no encontrado");

        // Crear un objeto Staff temporal para validar los datos
        var staffToValidate = new Staff
        {
            Name = request.Name,
            LastName = request.LastName,
            Ci = request.Ci,
            Age = request.Age,
            NumberPhone = request.NumberPhone,
            Role = request.Role,
            Password = user.Password,
            Id = userId,
        };

        // Validar los datos
        var error = Validaciones.ValidarStaff(staffToValidate);
        if (error != null)
            return (false, error);

        // Verificar si el nuevo CI ya está registrado por otro usuario
        var existingUserWithCi = await _repository.GetByCiAsync(request.Ci);
        if (existingUserWithCi != null && existingUserWithCi.Id != userId)
            return (false, "El CI ya está registrado por otro usuario.");

        // Actualizar los campos del usuario
        user.Name = request.Name;
        user.LastName = request.LastName;
        user.Ci = request.Ci;
        user.Age = request.Age;
        user.NumberPhone = request.NumberPhone;
        user.Role = request.Role;

        await _repository.UpdateAsync(user);
        return (true, "Datos actualizados con éxito");
    }

    public async Task<(bool Success, StaffResponse? Data, string Message)> GetByIdAsync(Guid userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null)
            return (false, null, "Usuario no encontrado");

        var response = new StaffResponse
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Ci = user.Ci,
            Age = user.Age,
            NumberPhone = user.NumberPhone,
            Role = user.Role,
        };

        return (true, response, "Usuario encontrado con éxito");
    }
}
