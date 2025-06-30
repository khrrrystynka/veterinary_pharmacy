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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
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