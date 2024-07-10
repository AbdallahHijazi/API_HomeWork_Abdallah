namespace API_HomeWork.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public String Phone { get; set; }
        public bool IsDelete { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();

    }
}

