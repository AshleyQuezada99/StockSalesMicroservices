using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using MediatR;
using Stock.Data;
using Stock.Repository;
using Stock.Repository.IRepository;


var builder = WebApplication.CreateBuilder(args);

// Configure the database connection
builder.Services.AddDbContext<ApplicationDbContextStock>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSqlStock"));
});
;

// Configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure repositories
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

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
