using ApiProcessamento.Config;
using ApiProcessamento.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=sensores.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Configuração do Swagger Técnica e Limpa
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SIMI API - Sistema de Monitoramento Industrial",
        Version = "v1.0",
        Description = "API centralizada para processamento de telemetria, validação de limites térmicos e persistência de dados em banco de dados SQLite."
    });

    try
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Aviso: Erro ao carregar XML: {ex.Message}");
    }
});

builder.Services.Configure<ApiConfig>(
    builder.Configuration.GetSection("ApiConfig"));

var app = builder.Build();

// 3. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIMI API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Documentação Técnica SIMI";
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();