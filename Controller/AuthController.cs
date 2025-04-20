using Authentication_Service.Models;
using Authentication_Service.Services;
using Authentication_Service.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(Staff request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request);
        if (token == null)
            return Unauthorized("Credenciales incorrectas");
        return Ok(new { token });
    }
}
