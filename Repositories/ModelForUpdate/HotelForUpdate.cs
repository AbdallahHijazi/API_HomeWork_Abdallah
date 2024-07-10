using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelForUpdate
{
    public class HotelForUpdate
    {
        [Required(ErrorMessage = "Please focus don't forget to send the name parameter")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
