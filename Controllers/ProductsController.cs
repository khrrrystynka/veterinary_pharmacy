using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetPharmacyApi.Data;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly VetPharmacyDbContext _context;

    public ProductsController(VetPharmacyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Отримати всі продукти з пагінацією (доступно ролям Doctor і Admin).
    /// </summary>
    /// <param name="pageNumber">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10)</param>
    /// <returns>Список продуктів з категоріями</returns>
    /// <response code="200">Повертає список продуктів</response>
    /// <response code="401">Якщо користувач не авторизований</response>
    [Authorize] // Doctor і Admin
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var products = await _context.Products
            .Include(p => p.Category)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return products;
    }

    /// <summary>
    /// Отримати всі продукти з фільтрацією, пагінацією та сортуванням за датою додавання (доступно ролям Doctor і Admin).
    /// </summary>
    /// <param name="search">Текст для пошуку в назві продукту</param>
    /// <param name="sortOrder">Порядок сортування за ArrivalDate: asc або desc (за замовчуванням desc)</param>
    /// <param name="pageNumber">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10)</param>
    /// <returns>Список продуктів</returns>
    [Authorize]
    [HttpGet("filter")]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(
        string? search = null,
        string? sortOrder = "desc",
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
    
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();
    
        // 🔍 Фільтрація по назві
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
        }
    
        // 🔃 Сортування за датою додавання
        query = sortOrder?.ToLower() == "asc"
            ? query.OrderBy(p => p.ArrivalDate)
            : query.OrderByDescending(p => p.ArrivalDate);
    
        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    
        return products;
    }

    /// <summary>
    /// Створити новий продукт (доступно тільки Admin).
    /// </summary>
    /// <param name="product">Дані нового продукту</param>
    /// <returns>Створений продукт</returns>
    /// <response code="201">Продукт успішно створений</response>
    /// <response code="400">Некоректні дані</response>
    /// <response code="401">Якщо користувач не авторизований або не має ролі Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(Product), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    /// <summary>
    /// Оновити продукт за ID (доступно тільки Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор продукту</param>
    /// <param name="product">Оновлені дані продукту</param>
    /// <returns>Порожня відповідь</returns>
    /// <response code="204">Продукт успішно оновлений</response>
    /// <response code="400">Некоректний запит або ID не співпадає</response>
    /// <response code="404">Продукт не знайдений</response>
    /// <response code="401">Якщо користувач не авторизований або не має ролі Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != product.Id) return BadRequest();
        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Видалити продукт за ID (доступно тільки Admin).
    /// </summary>
    /// <param name="id">Ідентифікатор продукту</param>
    /// <returns>Порожня відповідь</returns>
    /// <response code="204">Продукт успішно видалений</response>
    /// <response code="400">Некоректний запит</response>
    /// <response code="404">Продукт не знайдений</response>
    /// <response code="401">Якщо користувач не авторизований або не має ролі Admin</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
