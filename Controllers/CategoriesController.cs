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
    /// Отримати всі категорії з фільтрацією та пагінацією (доступно Doctor і Admin).
    /// </summary>
    /// <param name="search">Рядок для пошуку по назві</param>
    /// <param name="pageNumber">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10)</param>
    /// <returns>Список категорій</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(string? search = null, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
        }

        var categories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return categories;
    }

    /// <summary>
    /// Отримати категорію за ID.
    /// </summary>
    [Authorize]
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

        if (id != category.Id)
            return BadRequest();

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
