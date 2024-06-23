using API_HomeWork.DBContext;
using API_HomeWork.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_HomeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelsContext hotelsContext;

        public HotelsController(HotelsContext hotelsContext)
        {
            this.hotelsContext = hotelsContext;
        }

        [HttpGet("{hotelId}",Name = "GetHotel")]
        public ActionResult<Hotels> GetHotels(int hotelId)
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult<Hotels> AddHotels(Hotels hotels)
        {
            
            return CreatedAtRoute("GetHotel",new {},new Hotels); 
        }
    }
}
