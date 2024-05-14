using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Repository;
using Stock.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

//Conexion DB
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

// Add services to the container.

builder.Services.AddControllers();
//Settings AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Settings Repository
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
