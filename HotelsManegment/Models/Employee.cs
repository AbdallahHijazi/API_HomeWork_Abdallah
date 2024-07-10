namespace API_HomeWork.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Title { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsDelete { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
