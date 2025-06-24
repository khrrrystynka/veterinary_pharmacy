using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Додаємо Swagger для OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Додаємо контролери (щоб мати API)
builder.Services.AddControllers();

var app = builder.Build();

// Вмикаємо Swagger UI (без перевірки середовища для тесту)
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// Простий маршрут на корінь, щоб перевірити запуск
app.MapGet("/", () => "Вітаю! API VetPharmacy працює.");

app.Run();