using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VetPharmacyApi.Data;
using VetPharmacyApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 👇 Додаємо контролери, Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "VetPharmacy API",
        Version = "v1"
    });

    // Додаємо схему авторизації
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введіть токен у форматі: **Bearer {токен}**"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 👇 Додаємо контекст БД
builder.Services.AddDbContext<VetPharmacyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👇 Додаємо сервіс генерації JWT
builder.Services.AddScoped<JwtService>();

// 👇 Налаштування автентифікації через JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 👇 Авторизація (доступ по ролях)
builder.Services.AddAuthorization();

var app = builder.Build();

// 👇 Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 👇 Увімкнути Authentication та Authorization
app.UseAuthentication();
app.UseAuthorization();

// 👇 Контролери
app.MapControllers();

// 👇 Тестова домашня сторінка
app.MapGet("/", () => "Вітаю! API VetPharmacy працює.");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VetPharmacyDbContext>();
    DbSeeder.Seed(dbContext);
}

app.Run();