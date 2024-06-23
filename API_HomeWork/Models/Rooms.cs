namespace API_HomeWork.Models
{
    public class Rooms
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        public bool IsDelete { get; set; }
        public Status status { get; set; }

    }
    public enum Status
    {
        ready,
        dirty,
        outOfdoor,
        occupied
    }
}

