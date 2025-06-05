using Authentication_Service.Models;
using System.Text.RegularExpressions;

namespace Authentication_Service.Utils;

public static class Validaciones
{
    public static string? ValidarStaff(Staff staff)
    {
        if (staff.Name.Length > 80)
            return "El nombre no debe exceder 80 caracteres.";

        if (staff.LastName.Length > 80)
            return "El apellido no debe exceder 80 caracteres.";

        if (staff.Ci < 1000000 || staff.Ci > 99999999)
            return "El CI debe contener entre 7 y 8 dígitos.";

        if (!EsPasswordValida(staff.Password))
            return "La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial.";

        if (staff.Age < 16 || staff.Age > 70)
            return "La edad debe estar entre 16 y 70 años.";

        if (!Regex.IsMatch(staff.NumberPhone.ToString(), @"^[67]\d{7}$"))
            return "El número de celular debe tener 8 dígitos y comenzar con 6 o 7.";

        if (staff.Role != "ADMIN" && staff.Role != "EMPLEADO")
            return "El rol debe ser 'ADMIN' o 'EMPLEADO' en mayúsculas.";

        return null; // Todo correcto
    }

    private static bool EsPasswordValida(string password)
    {
        var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");
        return regex.IsMatch(password);
    }
}
