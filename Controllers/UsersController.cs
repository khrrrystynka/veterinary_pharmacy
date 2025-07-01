using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetPharmacyApi.Data;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly VetPharmacyDbContext _context;

    public UsersController(VetPharmacyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Отримати всіх користувачів (доступно лише Admin).
    /// </summary>
    /// <returns>Список користувачів</returns>
    /// <response code="200">Повертає список користувачів</response>
    /// <response code="401">Якщо користувач не авторизований або не Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppUser>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _context.Users.ToListAsync();
    }

    /// <summary>
    /// Отримати користувача за ID (доступно лише Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор користувача</param>
    /// <returns>Користувач</returns>
    /// <response code="200">Повертає користувача</response>
    /// <response code="400">Некоректний запит</response>
    /// <response code="404">Користувач не знайдений</response>
    /// <response code="401">Якщо користувач не авторизований або не Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppUser), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return user;
    }

    /// <summary>
    /// Створити нового користувача (дозволено анонімним, щоб можна було створити першого Admin).
    /// </summary>
    /// <param name="user">Дані користувача</param>
    /// <returns>Створений користувач</returns>
    /// <response code="201">Користувач успішно створений</response>
    /// <response code="400">Некоректні дані</response>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(AppUser), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AppUser>> CreateUser(AppUser user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var hasher = new PasswordHasher<AppUser>();
        user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    /// <summary>
    /// Оновити користувача за ID (доступно лише Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор користувача</param>
    /// <param name="user">Оновлені дані користувача</param>
    /// <returns>Порожня відповідь</returns>
    /// <response code="204">Користувач успішно оновлений</response>
    /// <response code="400">Некоректний запит або ID не співпадає</response>
    /// <response code="404">Користувач не знайдений</response>
    /// <response code="401">Якщо користувач не авторизований або не Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> UpdateUser(int id, AppUser user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != user.Id) return BadRequest();
        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Видалити користувача за ID (доступно лише Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор користувача</param>
    /// <returns>Порожня відповідь</returns>
    /// <response code="204">Користувач успішно видалений</response>
    /// <response code="400">Некоректний запит</response>
    /// <response code="404">Користувач не знайдений</response>
    /// <response code="401">Якщо користувач не авторизований або не Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
