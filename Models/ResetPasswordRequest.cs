namespace Authentication_Service.Models;

public class ResetPasswordRequest
{
    public int Ci { get; set; }
    public int NumerPhone { get; set; }
    public string NewPassword { get; set; } = string.Empty;
}
