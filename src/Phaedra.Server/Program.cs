using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using Phaedra.Server.Mappings;
using Phaedra.Server.Middlewares;
using Phaedra.Server.Models.DTO.User;
using Phaedra.Server.Models.Entities.Users;
using Phaedra.Server.Services;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DefaultDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.AddScoped<IDataService<User, UserDto, CreateUserDto, UpdateUserDto>, DataService<User, UserDto, CreateUserDto, UpdateUserDto>>();
builder.Services.AddAutoMapper(typeof(DomainToDtoMappingProfile));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtIssuer"] ?? throw new InvalidOperationException("JwtIssuer is missing"),
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSecret"] ?? throw new InvalidOperationException("JWT Secret Key is missing"))),
        };
    });
var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
