using Microsoft.EntityFrameworkCore;
using HotelStorageApi.Data;
using HotelStorageApi.Models;

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

app.UseDefaultFiles();          
app.UseStaticFiles();           

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.MapGet("/", () => Results.Redirect("/index.html"));
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelsDBContext>();
    context.Database.EnsureCreated();

    context.Hotels.RemoveRange(context.Hotels);
    context.HotelNames.RemoveRange(context.HotelNames);
    context.Cities.RemoveRange(context.Cities);
    context.SaveChanges();

    var hotelNamesList = new List<HotelName>
    {
        new HotelName { Name = "Grand Palace" },
        new HotelName { Name = "Seaside Resort" },
        new HotelName { Name = "Mountain View" },
        new HotelName { Name = "City Central" },
        new HotelName { Name = "Sunset Inn" },
        new HotelName { Name = "Royal Comfort" },
        new HotelName { Name = "Blue Ocean" },
        new HotelName { Name = "Green Park" },
        new HotelName { Name = "Star Plaza" },
        new HotelName { Name = "Golden Tulip" }
    };
    context.HotelNames.AddRange(hotelNamesList);

    var citiesList = new List<City>
    {
        new City { CityName = "Москва" },
        new City { CityName = "Санкт-Петербург" },
        new City { CityName = "Сочи" },
        new City { CityName = "Казань" },
        new City { CityName = "Екатеринбург" }
    };
    context.Cities.AddRange(citiesList);
    context.SaveChanges();

    var hotelsList = new List<Hotel>
    {
        new Hotel { HotelName = hotelNamesList[0], City = citiesList[0], Capacity = 120, StandardRoomPrice = 4500 },
        new Hotel { HotelName = hotelNamesList[1], City = citiesList[1], Capacity = 85,  StandardRoomPrice = 6200 },
        new Hotel { HotelName = hotelNamesList[2], City = citiesList[2], Capacity = 200, StandardRoomPrice = 7800 },
        new Hotel { HotelName = hotelNamesList[3], City = citiesList[3], Capacity = 45,  StandardRoomPrice = 3900 },
        new Hotel { HotelName = hotelNamesList[4], City = citiesList[4], Capacity = 110, StandardRoomPrice = 5100 },
        new Hotel { HotelName = hotelNamesList[5], City = citiesList[0], Capacity = 300, StandardRoomPrice = 8900 },
        new Hotel { HotelName = hotelNamesList[6], City = citiesList[1], Capacity = 60,  StandardRoomPrice = 7200 },
        new Hotel { HotelName = hotelNamesList[7], City = citiesList[2], Capacity = 95,  StandardRoomPrice = 5500 },
        new Hotel { HotelName = hotelNamesList[8], City = citiesList[3], Capacity = 150, StandardRoomPrice = 6400 },
        new Hotel { HotelName = hotelNamesList[9], City = citiesList[4], Capacity = 40,  StandardRoomPrice = 4100 }
    };
    context.Hotels.AddRange(hotelsList);
    context.SaveChanges();
}

app.Run();