using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sale.Data;
using Sale.Repository;
using Sale.Repository.IRepository;
using MediatR;
using Stock.HandlerRabbit;
using Stock.Repository.IRepository;
using Stock.Repository;
using Stock.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Database connection for sales
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

// Database connection for products
builder.Services.AddDbContext<ApplicationDbContextStock>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSqlStock"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

// Register MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddLogging();

// Register the repositories
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
// Register the handlers
builder.Services.AddScoped<INotificationHandler<StockUpdateEvent>, StockEventHandler>();

// Register RabbitMQService with a singleton
builder.Services.AddSingleton<RabbitMQService>(sp =>
{
    var hostname = "rabbitmq";
    var queueName = "test";
    return new RabbitMQService(hostname, queueName, sp);
});

// Add controllers to the container
builder.Services.AddControllers();

// Configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.Run();
