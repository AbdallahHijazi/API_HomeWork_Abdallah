using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelForCreate
{
    public class RoomForCreate
    {
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        public Status status { get; set; }
        public int HotelId { get; set; }
    }
    public enum Status
    {
        ready,
        dirty,
        outOfDoor,
        occupied
    }
}
