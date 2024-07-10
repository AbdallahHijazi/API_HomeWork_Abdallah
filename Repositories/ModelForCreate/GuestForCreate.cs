using API_HomeWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelForCreate
{
    public class GuestForCreate
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public String Phone { get; set; }
        public int HotelId { get; set; }
       
    }
}
