using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Sale.Data;
using Sale.Repository;
using Sale.Repository.IRepository;
using Microsoft.Extensions.Options;
using Stock.Entities;
using StockSale.RabbitMQ.Bus.BusRabbit;
using StockSale.RabbitMQ.Bus.Implement;
using MediatR;
using Stock.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Conexion DB
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//Settings RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { HostName = "rabbit-ash-web" };
    return factory.CreateConnection();
});
builder.Services.AddSingleton<IRabbitEventBus, RabbitEventBus>();
builder.Services.AddSingleton(new Dictionary<string, List<Type>>());
builder.Services.AddSingleton(new List<Type>());

builder.Services.AddTransient<IRabbitEventBus, RabbitEventBus>();
//Settings MediatR
builder.Services.AddMediatR(typeof(ISalesRepository).Assembly);
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
