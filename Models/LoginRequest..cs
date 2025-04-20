namespace Authentication_Service.Models;

public class LoginRequest
{
    public int Ci { get; set; }
    public string Password { get; set; } = string.Empty;
}
