using escafandra.services.Application.Interfaces;
using escafandra.services.Application.Services;
using escafandra.services.Infrastructure.Repositories.Interfaces;
using escafandra.services.Infrastructure.Repositories;
using escafandra.services.Infrastructure.UnitOfWork;
using escafandra.services.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Agregar servicios al contenedor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


builder.Services.AddControllers();

// Agregar servicios al contenedor
builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CNXProducrion"))); 

// Register services
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IPdfRepository, PdfRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplica las migraciones al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    context.Database.Migrate(); // Aplica migraciones pendientes
}

// Middleware de enrutamiento
app.UseRouting();
// Habilitar CORS
app.UseCors("AllowAll");

// Registrar middleware para el manejo de errores
app.UseMiddleware<escafandra.services.API.Middleware.ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
