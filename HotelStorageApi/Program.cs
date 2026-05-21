using Microsoft.EntityFrameworkCore;
using HotelStorageApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<HotelsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Эти две строки гарантируют, что при обращении к корню "/" будет отдан index.html
app.UseDefaultFiles();          // ищет index.html, default.html и т.д.
app.UseStaticFiles();           // отдаёт статические файлы из wwwroot

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Дополнительное перенаправление на index.html (страховка)
app.MapGet("/", () => Results.Redirect("/index.html"));

app.Run();