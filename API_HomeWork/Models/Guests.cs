namespace API_HomeWork.Models
{
    public class Guests
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public String Phone { get; set; }
        public bool IsDelete { get; set; }
    }
}
