using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VetPharmacyApi.DTOs;
using VetPharmacyApi.Services;
using VetPharmacyApi.Data;
using Microsoft.EntityFrameworkCore;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly VetPharmacyDbContext _context;
    private readonly JwtService _jwtService;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public AuthController(VetPharmacyDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Логін користувача та отримання JWT токена.
    /// </summary>
    /// <param name="request">Дані для авторизації (username, password)</param>
    /// <returns>JWT токен у разі успіху</returns>
    /// <response code="200">Успішний вхід, повертається токен</response>
    /// <response code="400">Некоректний запит</response>
    /// <response code="401">Невірний логін або пароль</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
            return Unauthorized();

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result != PasswordVerificationResult.Success)
            return Unauthorized();

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }
}