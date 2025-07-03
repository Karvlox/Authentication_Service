using Authentication_Service.Models;

namespace Authentication_Service.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginRequest request);
    Task<(bool Success, string Message)> RegisterAsync(Staff staff);
    Task<(bool Success, string Message)> ChangePasswordAsync(ChangePasswordRequest request);
    Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordRequest request);
    Task<(bool Success, string Message)> UpdateAsync(Guid userId, UpdateStaffRequest request);
    Task<(bool Success, StaffResponse? Data, string Message)> GetByIdAsync(Guid userId);
}
