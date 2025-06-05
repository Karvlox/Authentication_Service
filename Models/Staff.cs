using System.ComponentModel.DataAnnotations;

namespace Authentication_Service.Models;

public class Staff
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public int Ci { get; set; }
    public required string Password { get; set; }
    public int Age { get; set; }
    public int NumerPhone { get; set; }
    public required string Role { get; set; }
}
