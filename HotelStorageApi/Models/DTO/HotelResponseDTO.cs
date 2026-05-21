namespace HotelStorageApi.Models.DTO
{
    public class HotelResponseDTO
    {
        public int Id { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal StandardRoomPrice { get; set; }
    }
}