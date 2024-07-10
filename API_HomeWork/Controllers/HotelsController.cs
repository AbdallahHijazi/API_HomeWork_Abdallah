using API_HomeWork.Models;
using AutoMapper;
using HotelDataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.ModelForCreate;
using Repository.ModelForCreate;
using Repository.ModelForUpdate;
using Repository.Repositories;

namespace API_HomeWork.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    //[AllowAnonymous]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelContext hotelsContext;
        private readonly IRepository<Hotel> hotelRepository;
        private readonly ILogger<HotelsController> logger;
        private readonly IMapper mapper;

        public HotelsController(HotelContext hotelsContext
                                , IRepository<Hotel> hotelRepository
                                , ILogger<HotelsController> logger
                                , IMapper mapper)
        {
            this.hotelsContext = hotelsContext;
            this.hotelRepository = hotelRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<Hotel>> AddHotel(HotelForCreate hotel)
        {
            try
            {
                // إنشاء كائن جديد من النوع Hotel وتعيين قيمه
                Hotel hotels = new Hotel()
                {
                    Name = hotel.Name,
                    Email = hotel.Email,
                    Phone = hotel.Phone,
                    Address = hotel.Address,
                };

                // إضافة الكائن الجديد إلى المستودع وحفظ التغييرات
                hotelRepository.Add(hotels);
                hotelsContext.SaveChanges();

                // إعادة الكائن المضاف مع حالة نجاح
                return CreatedAtAction("GetHotel", new { hotelId = hotels.Id }, hotels);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء وإعادة حالة خطأ 500
                this.logger.LogCritical("Unhandled exception : error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [AllowAnonymous]
        [HttpGet("{hotelId}", Name = "GetHotel")]
        public async Task<ActionResult<Hotel>> GetHotel(int hotelId)
        {
            try
            {
                // الحصول على الفندق من المستودع باستخدام معرّف الفندق
                var hotel = await hotelRepository.Get(hotelId);
                if (hotel == null || hotel.IsDelete == true)
                {
                    // تسجيل تحذير في حالة عدم وجود الفندق وإعادة حالة 404
                    logger.LogWarning($"This hotel with ID {hotelId} does not exist. Please try again later");
                    return NotFound();
                }

                // إعادة الفندق مع حالة نجاح
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء وإعادة حالة خطأ 500
                this.logger.LogCritical("Unhandled exception : error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Hotel>>> GetHotelsWithOut()
        {
            try
            {
                // الحصول على قائمة بجميع الفنادق من المستودع
                var hotel = await hotelRepository.GetAll();

                // تحويل قائمة الفنادق إلى قائمة من النوع HotelWithOutEnyThing
                var hotelWithOut = mapper.Map<List<HotelWithOutEnyThing>>(hotel);

                // إعادة القائمة المحولة مع حالة نجاح
                return Ok(hotelWithOut);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء وإعادة حالة خطأ 500
                this.logger.LogCritical("Unhandled exception : error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [HttpPut("{hotelId}")]
        public async Task<ActionResult<Hotel>> UpdateHotel(int hotelId, HotelForUpdate hotel)
        {
            try
            {
                // تحديث معلومات الفندق باستخدام معرّف الفندق
                var Hotel = await hotelRepository.Update(hotelId);
                if (Hotel == null || Hotel.IsDelete == true)
                {
                    // تسجيل تحذير في حالة عدم وجود الفندق وإعادة حالة 404
                    logger.LogWarning($"This hotel with ID {hotelId} does not exist. Please try again later");
                    return NotFound();
                }

                // تحديث تفاصيل الفندق
                Hotel.Name = hotel.Name;
                Hotel.Phone = hotel.Phone;
                Hotel.Email = hotel.Email;
                Hotel.Address = hotel.Address;

                // حفظ التغييرات في قاعدة البيانات
                hotelsContext.SaveChanges();

                // إعادة حالة نجاح بدون محتوى
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء وإعادة حالة خطأ 500
                this.logger.LogCritical("Unhandled exception : error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [HttpDelete("{hotelId}")]
        public ActionResult<Hotel> DeleteHotel(int hotelId)
        {
            try
            {
                // البحث عن الفندق باستخدام معرّف الفندق
                var hotel = hotelsContext.Hotels.FirstOrDefault(h => h.Id == hotelId);
                if (hotel == null)
                {
                    // تسجيل تحذير في حالة عدم وجود الفندق وإعادة حالة 404
                    logger.LogWarning($"This hotel with ID {hotelId} does not exist. Please try again later");
                    return NotFound();
                }

                // التحقق من ما إذا كان الفندق قد تم حذفه سابقاً
                if (hotel.IsDelete == true)
                {
                    // إعادة حالة خطأ إذا كان الفندق محذوف سابقاً
                    return BadRequest("This hotel has already been deleted");
                }

                // حذف الفندق من المستودع
                hotelRepository.Delete(hotelId);

                // إعادة حالة نجاح بدون محتوى
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء وإعادة حالة خطأ 500
                this.logger.LogCritical("Unhandled exception : error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }


    }
}
