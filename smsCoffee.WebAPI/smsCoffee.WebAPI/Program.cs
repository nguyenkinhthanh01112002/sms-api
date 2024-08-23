using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CoffeeDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
 
// Add services to the container.

builder.Services.AddControllers();
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
