namespace API_HomeWork.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Employee> Employees { get; set; } =  new List<Employee>();
        public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}
