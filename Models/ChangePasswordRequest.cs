namespace Authentication_Service.Models;

public class ChangePasswordRequest
{
    public int Ci { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
