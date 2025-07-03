using System;

namespace Authentication_Service.Models;

public class StaffResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Ci { get; set; }
    public int Age { get; set; }
    public int NumberPhone { get; set; }
    public string Role { get; set; } = string.Empty;
}