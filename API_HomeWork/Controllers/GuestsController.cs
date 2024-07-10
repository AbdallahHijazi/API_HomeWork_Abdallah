using API_HomeWork.Models;
using HotelDataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.ModelForCreate;
using Repository.ModelForUpdate;
using Repository.Repositories;

namespace API_HomeWork.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly HotelContext context;
        private readonly IRepository<Guest> guestRepository;
        private readonly ILogger<GuestsController> logger;

        public GuestsController(HotelContext context
                                , IRepository<Guest> guestRepository
                                , ILogger<GuestsController> logger)
        {
            this.context = context;
            this.guestRepository = guestRepository;
            this.logger = logger;
        }
        [HttpPost]
        // هذه الدالة تضيف نزيل جديد إلى قاعدة البيانات
        public async Task<ActionResult<Guest>> AddGuest(GuestForCreate guest)
        {
            try
            {
                // إنشاء كائن جديد من نوع Guest وإعداد خصائصه بالقيم المستلمة
                Guest newGuest = new Guest()
                {
                    FirstName = guest.FirstName,
                    LastName = guest.Lastname,
                    DOB = guest.DOB,
                    Email = guest.Email,
                    Phone = guest.Phone,
                    HotelId = guest.HotelId
                };
                // إضافة النزيل الجديد إلى المستودع
                guestRepository.Add(newGuest);
                // إرجاع كائن النزيل الجديد مع حالة Created
                return CreatedAtAction("GetHotel", new { newGuest.Id }, newGuest);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حال حدوث استثناء
                this.logger.LogCritical("Unhandled exception: error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [AllowAnonymous]
        [HttpGet("{guestId}", Name = "GetGuest")]
        // هذه الدالة تحصل على بيانات نزيل محدد باستخدام معرفه
        public async Task<ActionResult<Guest>> GetGuest(int guestId)
        {
            try
            {
                // الحصول على النزيل من المستودع
                var guest = await guestRepository.Get(guestId);
                // التحقق من وجود النزيل وعدم حذفه
                if (guest == null || guest.IsDelete == true)
                {
                    logger.LogWarning($"This guest with ID {guestId} does not exist. Please try again later");
                    return NotFound();
                }
                // إرجاع بيانات النزيل مع حالة OK
                return Ok(guest);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حال حدوث استثناء
                this.logger.LogCritical("Unhandled exception: error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        // هذه الدالة تحصل على قائمة بجميع النزلاء
        public async Task<ActionResult<List<Guest>>> GetGuests()
        {
            try
            {
                // الحصول على قائمة النزلاء من المستودع
                var guests = await guestRepository.GetAll();
                // التحقق من وجود النزلاء
                if (guests == null)
                {
                    logger.LogWarning($"This guests do not exist. Please try again later");
                    return NotFound();
                }
                // إرجاع قائمة النزلاء مع حالة OK
                return Ok(guests);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حال حدوث استثناء
                this.logger.LogCritical("Unhandled exception: error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [HttpPut("{guestId}")]
        // هذه الدالة تحدث بيانات نزيل موجود بناءً على معرفه
        public async Task<ActionResult<Guest>> UpdateGuest(int guestId, GuestForUpdate guest)
        {
            try
            {
                // الحصول على النزيل المراد تحديثه من المستودع
                var Guest = await guestRepository.Update(guestId);
                // التحقق من وجود النزيل وعدم حذفه
                if (Guest == null || Guest.IsDelete == true)
                {
                    logger.LogWarning($"This guest with ID {guestId} does not exist. Please try again later");
                    return NotFound();
                }
                // تحديث خصائص النزيل
                guest.FirstName = guest.FirstName;
                guest.Lastname = guest.Lastname;
                guest.Email = guest.Email;
                guest.Phone = guest.Phone;
                guest.DOB = guest.DOB;

                // حفظ التغييرات في قاعدة البيانات
                context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حال حدوث استثناء
                this.logger.LogCritical("Unhandled exception: error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

        [HttpDelete("{guestId}")]
        // هذه الدالة تحذف نزيل بناءً على معرفه
        public ActionResult<Guest> DeleteGuest(int guestId)
        {
            try
            {
                // الحصول على النزيل المراد حذفه من قاعدة البيانات
                var guest = context.Guests.FirstOrDefault(g => g.Id == guestId);
                // التحقق من وجود النزيل
                if (guest == null)
                {
                    logger.LogWarning($"This guest with ID {guestId} does not exist. Please try again later");
                    return NotFound();
                }
                // التحقق من أن النزيل لم يتم حذفه سابقاً
                if (guest.IsDelete == true)
                {
                    return BadRequest("This guest has already been deleted");
                }
                // حذف النزيل من المستودع
                guestRepository.Delete(guestId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حال حدوث استثناء
                this.logger.LogCritical("Unhandled exception: error happened", ex);
                return StatusCode(500, "An error occurred, please try again later");
            }
        }

    }
}

