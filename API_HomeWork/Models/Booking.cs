namespace API_HomeWork.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckInAt { get; set; }
        public DateTime CheckOutAt { get; set; }
        public double Price { get; set; }
        public bool IsDelete { get; set; }
       
    }
}
