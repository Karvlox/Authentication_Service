using System.ComponentModel.DataAnnotations;

namespace Authentication_Service.Models;

public class UpdateStaffRequest
{
    [Required]
    [StringLength(80, ErrorMessage = "El nombre no debe exceder 80 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(80, ErrorMessage = "El apellido no debe exceder 80 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Range(1000000, 99999999, ErrorMessage = "El CI debe contener entre 7 y 8 dígitos.")]
    public int Ci { get; set; }

    [Required]
    [Range(16, 70, ErrorMessage = "La edad debe estar entre 16 y 70 años.")]
    public int Age { get; set; }

    [Required]
    [RegularExpression(@"^[67]\d{7}$", ErrorMessage = "El número de celular debe tener 8 dígitos y comenzar con 6 o 7.")]
    public int NumberPhone { get; set; }

    [Required]
    [RegularExpression(@"^(ADMIN|EMPLEADO)$", ErrorMessage = "El rol debe ser 'ADMIN' o 'EMPLEADO' en mayúsculas.")]
    public string Role { get; set; } = string.Empty;
}