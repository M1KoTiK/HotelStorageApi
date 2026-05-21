using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelStorageApi.Data;
using HotelStorageApi.Models.DTO;
using HotelStorageApi.Models;

namespace HotelStorageApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly HotelsDBContext _context;

    public HotelsController(HotelsDBContext context)
    {
        _context = context;
    }

    // GET: api/hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelResponseDTO>>> GetHotels()
    {
        var hotels = await _context.Hotels
        .Include(h => h.HotelName)
            .Include(h => h.City)
            .Select(h => new HotelResponseDTO
            {
                Id = h.Id,
                HotelName = h.HotelName!.Name,
                City = h.City!.CityName,
                Capacity = h.Capacity,
                StandardRoomPrice = h.StandardRoomPrice
            })
            .ToListAsync();
        return Ok(hotels);
    }

    // POST: api/hotels
    [HttpPost]
    public async Task<ActionResult> AddHotel([FromBody] HotelCreateDto dto)
    {
        // Поиск или создание названия отеля
        var hotelName = await _context.HotelNames
            .FirstOrDefaultAsync(h => h.Name == dto.HotelName);
        if (hotelName == null)
        {
            hotelName = new HotelName { Name = dto.HotelName };
            _context.HotelNames.Add(hotelName);
            await _context.SaveChangesAsync();
        }

        // Поиск или создание города
        var city = await _context.Cities
            .FirstOrDefaultAsync(c => c.CityName == dto.City);
        if (city == null)
        {
            city = new City { CityName = dto.City };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
        }

        var hotel = new Hotel
        {
            HotelNameId = hotelName.Id,
            CityId = city.Id,
            Capacity = dto.Capacity,
            StandardRoomPrice = dto.StandardRoomPrice
        };
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Отель добавлен", id = hotel.Id });
    }

    // DELETE: api/hotels/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null) return NotFound();

        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Удалено" });
    }

    // GET: api/hotels/search?q=текст
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<HotelResponseDTO>>> Search(string q)
    {
        var hotels = await _context.Hotels
            .Include(h => h.HotelName)
            .Include(h => h.City)
            .Where(h => h.HotelName!.Name.Contains(q) || h.City!.CityName.Contains(q))
            .Select(h => new HotelResponseDTO
            {
                Id = h.Id,
                HotelName = h.HotelName!.Name,
                City = h.City!.CityName,
                Capacity = h.Capacity,
                StandardRoomPrice = h.StandardRoomPrice
            })
            .ToListAsync();
        return Ok(hotels);
    }

    // GET: api/hotels/sort?field=hotelname&order=asc
    [HttpGet("sort")]
    public async Task<ActionResult<IEnumerable<HotelResponseDTO>>> Sort(string field, string order = "asc")
    {
        var query = _context.Hotels
            .Include(h => h.HotelName)
            .Include(h => h.City)
            .Select(h => new HotelResponseDTO
            {
                Id = h.Id,
                HotelName = h.HotelName!.Name,
                City = h.City!.CityName,
                Capacity = h.Capacity,
                StandardRoomPrice = h.StandardRoomPrice
            });

        query = field.ToLower() switch
        {
            "hotelname" => order == "asc" ? query.OrderBy(h => h.HotelName) : query.OrderByDescending(h => h.HotelName),
            "city" => order == "asc" ? query.OrderBy(h => h.City) : query.OrderByDescending(h => h.City),
            "capacity" => order == "asc" ? query.OrderBy(h => h.Capacity) : query.OrderByDescending(h => h.Capacity),
            "price" => order == "asc" ? query.OrderBy(h => h.StandardRoomPrice) : query.OrderByDescending(h => h.StandardRoomPrice),
            _ => query.OrderBy(h => h.Id)
        };
        return Ok(await query.ToListAsync());
    }

    // GET: api/hotels/countByCapacity?min=50&max=200
    [HttpGet("countByCapacity")]
    public async Task<IActionResult> CountByCapacity(int min, int max)
    {
        var count = await _context.Hotels.CountAsync(h => h.Capacity >= min && h.Capacity <= max);
        return Ok(new { count });
    }
}