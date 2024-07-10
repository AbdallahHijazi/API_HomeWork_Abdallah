namespace Repository.ModelForUpdate
{
    public class RoomForUpdate
    {
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        public Status status { get; set; }
    }
    public enum Status
    {
        ready,
        dirty,
        outOfDoor,
        occupied
    }
}
