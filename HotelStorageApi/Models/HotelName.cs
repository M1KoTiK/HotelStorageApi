using System;
using System.Collections.Generic;

namespace HotelStorageApi.Models;

public partial class HotelName
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
