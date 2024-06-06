using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sale.Data;
using Sale.Repository;
using Sale.Repository.IRepository;
using MediatR;
using Stock.HandlerRabbit;


var builder = WebApplication.CreateBuilder(args);

// Conexion DB
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

builder.Services.AddSingleton<RabbitMQService>(sp =>
{
    // Configura RabbitMQService aquí
    var hostname = "rabbitmq";
    var queueName = "test";
    return new RabbitMQService(hostname, queueName);
});
// Add services to the container.
builder.Services.AddControllers();
// Configuración de AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Configuración de Repository
builder.Services.AddScoped<ISalesRepository, SalesRepository>();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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