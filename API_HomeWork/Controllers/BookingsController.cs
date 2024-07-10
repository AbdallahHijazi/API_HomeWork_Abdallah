//using API_HomeWork.Models;
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
    public class BookingsController : ControllerBase
    {
        // الحقول الخاصة المخزنة للاستخدام داخل وحدة التحكم
        private readonly HotelContext context;
        private readonly IRepository<Booking> bookingRepository;
        private readonly ILogger<BookingsController> logger;

        // المنشئ لوحدة التحكم BookingsController
        public BookingsController(HotelContext context
                                  , IRepository<Booking> bookingRepository
                                  , ILogger<BookingsController> logger)
        {
            // تهيئة الحقول الخاصة بالقيم الممررة
            this.context = context;
            this.bookingRepository = bookingRepository;
            this.logger = logger;
        }
        // إضافة حجز جديد
        [HttpPost]
        public async Task<ActionResult<Booking>> AddBooking(BookingForCreate booking)
        {
            try
            {
                // إنشاء حجز جديد بناءً على المدخلات
                Booking newBooking = new Booking()
                {
                    CheckInAt = booking.CheckInAt,
                    CheckOutAt = booking.CheckOutAt,
                    Price = booking.Price,
                    EmployeeId = booking.EmployeeId,
                    RoomId = booking.RoomId,
                    GuestId = booking.GuestId
                };

                // إضافة الحجز الجديد إلى المستودع
                bookingRepository.Add(newBooking);

                // إعادة النتيجة مع حالة النجاح CreatedAtAction
                return CreatedAtAction("GetBooking", new { bookingId = newBooking.Id }, newBooking);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إعادة حالة الخطأ 500 مع رسالة للمستخدم
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        // استرجاع حجز بناءً على معرف الحجز
        [AllowAnonymous]
        [HttpGet("{bookingId}", Name = "GetBooking")]
        public async Task<ActionResult<Booking>> GetBooking(int bookingId)
        {
            try
            {
                // الحصول على الحجز من المستودع بناءً على معرف الحجز
                var booking = await bookingRepository.Get(bookingId);

                // التحقق من وجود الحجز وعدم حذفه
                if (booking == null || booking.IsDelete == true)
                {
                    logger.LogWarning($"This booking with ID {bookingId} does not exist. Please try again later");
                    return NotFound();
                }

                // إعادة الحجز مع حالة النجاح 200
                return Ok(booking);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إعادة حالة الخطأ 500 مع رسالة للمستخدم
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        // استرجاع جميع الحجوزات
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookings()
        {
            try
            {
                // الحصول على جميع الحجوزات من المستودع
                var bookings = await bookingRepository.GetAll();

                // التحقق من وجود الحجوزات
                if (bookings == null)
                {
                    logger.LogWarning($"This bookings does not exist. Please try again later");
                    return NotFound();
                }

                // إعادة الحجوزات مع حالة النجاح 200
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إعادة حالة الخطأ 500 مع رسالة للمستخدم
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        // تحديث حجز بناءً على معرف الحجز
        [HttpPut("{bookingId}")]
        public async Task<ActionResult<Booking>> UpdateBooking(int bookingId, BookingForUpdate booking)
        {
            try
            {
                // تحديث الحجز في المستودع بناءً على معرف الحجز
                var Booking = await bookingRepository.Update(bookingId);

                // التحقق من وجود الحجز وعدم حذفه
                if (Booking == null || Booking.IsDelete == true)
                {
                    logger.LogWarning($"This booking with ID {bookingId} does not exist. Please try again later");
                    return NotFound();
                }

                // تحديث تفاصيل الحجز
                booking.CheckInAt = booking.CheckInAt;
                booking.CheckOutAt = booking.CheckOutAt;
                booking.Price = booking.Price;

                // حفظ التغييرات في السياق
                context.SaveChanges();

                // إعادة حالة النجاح NoContent
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إعادة حالة الخطأ 500 مع رسالة للمستخدم
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        // حذف حجز بناءً على معرف الحجز
        [HttpDelete("{bookingId}")]
        public ActionResult<Booking> DeleteBooking(int bookingId)
        {
            try
            {
                // البحث عن الحجز في السياق بناءً على معرف الحجز
                var booking = context.Bookings.FirstOrDefault(b => b.Id == bookingId);

                // التحقق من وجود الحجز
                if (booking == null)
                {
                    logger.LogWarning($"This booking with ID {bookingId} does not exist. Please try again later");
                    return NotFound();
                }

                // التحقق من عدم حذف الحجز مسبقاً
                if (booking.IsDelete == true)
                {
                    return BadRequest("This booking has already been deleted");
                }

                // حذف الحجز من المستودع
                bookingRepository.Delete(bookingId);

                // إعادة حالة النجاح NoContent
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إعادة حالة الخطأ 500 مع رسالة للمستخدم
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
    }
}
