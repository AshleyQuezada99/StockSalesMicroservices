using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using MediatR;
using Stock.Data;
using Stock.HandlerRabbit;
using Stock.Repository;
using Stock.Repository.IRepository;
using StockSale.RabbitMQ.Bus.BusRabbit;
using StockSale.RabbitMQ.Bus.EventsQueue;
using StockSale.RabbitMQ.Bus.Implement;

var builder = WebApplication.CreateBuilder(args);

// Configure the database connection
builder.Services.AddDbContext<ApplicationDbContextStock>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSqlStock"));
});

// Configure RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { HostName = "rabbit-ash-web" };
    return factory.CreateConnection();
});
builder.Services.AddTransient<IRabbitEventBus, RabbitEventBus>();
builder.Services.AddSingleton(new Dictionary<string, List<Type>>());
builder.Services.AddSingleton(new List<Type>());
builder.Services.AddTransient<IEventHandler<UpdateStockEvent>, StockEventHandler>();

// Configure MediatR
builder.Services.AddMediatR(typeof(IProductsRepository).Assembly);

// Configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure repositories
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Start the RabbitMQ consumer
var eventBus = app.Services.GetRequiredService<IRabbitEventBus>();
eventBus.Subscribe<UpdateStockEvent, StockEventHandler>();

app.Run();
