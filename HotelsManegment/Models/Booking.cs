namespace API_HomeWork.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckInAt { get; set; }
        public DateTime CheckOutAt { get; set; }
        public double Price { get; set; }
        public bool IsDelete { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }

    }
}
