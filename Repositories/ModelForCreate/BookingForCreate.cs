using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelForCreate
{
    public class BookingForCreate
    {
        public DateTime CheckInAt { get; set; }
        public DateTime CheckOutAt { get; set; }
        public double Price { get; set; }
        public int EmployeeId { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
    }
}
