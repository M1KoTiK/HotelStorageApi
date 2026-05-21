using System;
using System.Collections.Generic;

namespace HotelStorageApi.Models;

public partial class Hotel
{
    public int Id { get; set; }

    public int HotelNameId { get; set; }

    public int CityId { get; set; }

    public int Capacity { get; set; }

    public decimal StandardRoomPrice { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual HotelName HotelName { get; set; } = null!;
}
