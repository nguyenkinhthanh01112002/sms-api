using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.DTOs.Common;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Middleware;
using smsCoffee.WebAPI.Models;
using smsCoffee.WebAPI.Services;
using smsCoffee.WebAPI.Services.AuthService;
using smsCoffee.WebAPI.Services.CategoryServices;
using smsCoffee.WebAPI.Services.CommonService;
using smsCoffee.WebAPI.Services.HangfireServices;
using smsCoffee.WebAPI.Services.ProductServices;
using smsCoffee.WebAPI.Services.RoleServices;
using System.Security.Cryptography;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

//Register DatabaseDefault
builder.Services.AddDbContext<CoffeeDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));
// Thêm mới cho Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DbContext"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Thêm Hangfire Server
// 2. Thêm Hangfire Server
/*builder.Services.AddHangfireServer(options =>
{
    *//*options.ServerName = $"coffee-dev-{Environment.MachineName}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
    options.WorkerCount = builder.Environment.IsDevelopment() ? 5 : Environment.ProcessorCount * 5;
    options.Queues = new[] { "critical", "default", "notifications" };
    options.SchedulePollingInterval = TimeSpan.FromSeconds(15);*//*

});*/

builder.Services.AddHangfireServer();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<CoffeeDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddIdentityCore<AppUser>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);
// Add services to the container.
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .Select(e => new ValidationError
            {
                Field = e.Key,
                Message = e.Value?.Errors.First().ErrorMessage ?? "Invalid input value"
            })
            .ToList();

        var response = new ApiResponse<List<ValidationError>>
        {
            Status = false,
            Path = context.HttpContext.Request.Path,
            Message = "Validation failed",
            StatusCode = StatusCodes.Status400BadRequest,
            Data = errors,
            Timestamp = DateTime.UtcNow
        };

        /*return new BadRequestObjectResult(response);*/
        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    };
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
    // Custom Auth Error Response
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            var response = new ApiResponse<object>
            {
                Status = false,
                Path = context.Request.Path,
                Message = "Unauthorized access",
                StatusCode = StatusCodes.Status401Unauthorized,
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        },

        OnAuthenticationFailed = async context =>
        {
            var response = new ApiResponse<object>
            {
                Status = false,
                Path = context.Request.Path,
                Message = context.Exception is SecurityTokenExpiredException
                    ? "Token has expired"
                    : "Authentication failed",
                StatusCode = StatusCodes.Status401Unauthorized,
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
    };
});
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

// 3. Custom Authorization Handler
builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // URL của frontend
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
// Cách 1: Sử dụng RouteOptions để lowercase tất cả các routes
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true; // Tùy chọn: lowercase cả query string
});

var app = builder.Build();
// 5. Configure Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Coffee Shop Jobs",
    DisplayStorageConnectionString = false
});

app.UseMiddleware<ExceptionHandlingMiddleware>();
// Sử dụng CORS
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// 6. Khởi tạo Recurring Jobs khi ứng dụng starts
try
{
    using (var scope = app.Services.CreateScope())
    {
        Console.WriteLine("Bắt đầu lấy service"); // Debug point 1
        var backgroundJobService = scope.ServiceProvider.GetRequiredService<IBackgroundJobService>();
        Console.WriteLine("Đã lấy được service"); // Debug point 2
        backgroundJobService.InitializeRecurringJobs();
        Console.WriteLine("Đã khởi tạo jobs"); // Debug point 3
    }
}
catch (Exception ex)
{
    // Log error
    Console.WriteLine($"Error initializing recurring jobs: {ex.Message}");
}
// 3. Configure Endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});





app.Run();

