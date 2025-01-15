using Microsoft.EntityFrameworkCore;
using Phaedra.Server.Data;
using Phaedra.Server.Middlewares;
using Phaedra.Server.Repositories;
using Phaedra.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DefaultDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IDataService<>), typeof(DataService<>));
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
