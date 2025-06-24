using Microsoft.AspNetCore.Mvc;

namespace VetPharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        // Просто повертаємо тестовий список товарів
        var products = new[]
        {
            new { Id = 1, Name = "Вітаміни для собак", Quantity = 20 },
            new { Id = 2, Name = "Антибактеріальні таблетки", Quantity = 15 }
        };
        return Ok(products);
    }
}