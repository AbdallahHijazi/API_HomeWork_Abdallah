namespace API_HomeWork.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        public bool IsDelete { get; set; }
        public Status status { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
    public enum Status
    {
        ready,
        dirty,
        outOfdoor,
        occupied
    }
}

