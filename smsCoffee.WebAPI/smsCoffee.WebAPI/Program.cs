using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Services;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
byte[] scretBytes = new byte[64];
using(var random = RandomNumberGenerator.Create())
{
    random.GetBytes(scretBytes);
}
string scretKey = Convert.ToBase64String(scretBytes);

builder.Services.AddDbContext<CoffeeDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "FreeTrained",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(scretKey))

    };
});
//builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<CoffeeDbContext>().AddDefaultTokenProviders();
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
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
app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
