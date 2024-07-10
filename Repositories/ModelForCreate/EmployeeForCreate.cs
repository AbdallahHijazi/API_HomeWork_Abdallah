using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelForCreate
{
    public class EmployeeForCreate
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Title { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public int HotelId { get; set; }
    }
}
