using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Добавление CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

// Добавление HttpClient
builder.Services.AddHttpClient();

// Регистрация сервисов и маппинга
builder.Services.AddScoped<IDadataService, DadataService>();
builder.Services.AddAutoMapper(typeof(Program)); // или typeof(AddressProfile)

// Добавление контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Использование CORS
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
