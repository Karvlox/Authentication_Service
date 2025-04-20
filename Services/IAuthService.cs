using Authentication_Service.Models;

namespace Authentication_Service.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginRequest request);
    Task<(bool Success, string Message)> RegisterAsync(Staff staff);
}
