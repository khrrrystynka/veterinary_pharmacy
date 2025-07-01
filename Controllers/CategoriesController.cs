using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetPharmacyApi.Data;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly VetPharmacyDbContext _context;

    public CategoriesController(VetPharmacyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Отримати всі категорії з пагінацією (доступно ролям Doctor і Admin).
    /// </summary>
    /// <param name="pageNumber">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10)</param>
    /// <returns>Список категорій</returns>
    /// <response code="200">Повертає список категорій</response>
    /// <response code="401">Якщо користувач не авторизований</response>
    [Authorize] // Doctor і Admin
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var categories = await _context.Categories
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return categories;
    }

    /// <summary>
    /// Отримати категорію за ID (доступно ролям Doctor і Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор категорії</param>
    /// <returns>Категорія</returns>
    /// <response code="200">Повертає категорію</response>
    /// <response code="400">Некоректний запит (ModelState не валідний)</response>
    /// <response code="404">Категорія не знайдена</response>
    /// <response code="401">Якщо користувач не авторизований</response>
    [Authorize] // Doctor і Admin
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Category), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return category;
    }

    /// <summary>
    /// Створити нову категорію (доступно тільки Admin).
    /// </summary>
    /// <param name="category">Дані нової категорії</param>
    /// <returns>Створена категорія</returns>
    /// <response code="201">Категорія успішно створена</response>
    /// <response code="400">Некоректні дані</response>
    /// <response code="401">Якщо користувач не авторизований або не має ролі Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(Category), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    /// <summary>
    /// Оновити категорію за ID (доступно тільки Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор категорії</param>
    /// <param name="category">Оновлені дані категорії</param>
    /// <returns>Порожня відповідь</returns>
    /// <response code="204">Категорія успішно оновлена</response>
    /// <response code="400">Некоректний запит або ID не співпадає</response>
    /// <response code="404">Категорія не знайдена</response>
    /// <response code="401">Якщо користувач не авторизований або не має ролі Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != category.Id) return BadRequest();
        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categories.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Видалити категорію за ID (доступно тільки Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор категорії</param>
    /// <returns>Порожня відповідь</returns>
    /// <response code="204">Категорія успішно видалена</response>
    /// <response code="400">Некоректний запит</response>
    /// <response code="404">Категорія не знайдена</response>
    /// <response code="401">Якщо користувач не авторизований або не має ролі Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
