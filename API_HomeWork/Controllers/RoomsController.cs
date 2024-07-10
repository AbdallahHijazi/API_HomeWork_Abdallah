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
    public class RoomsController : ControllerBase
    {
        private readonly HotelContext context;
        private readonly ILogger<RoomsController> logger;
        private readonly IRepository<Room> roomRepository;

        public RoomsController(HotelContext context
                               , IRepository<Room> roomRepository
                               , ILogger<RoomsController> logger)
        {
            this.context = context;
            this.logger = logger;
            roomRepository = roomRepository;
        }
        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom(RoomForCreate room)
        {
            try
            {
                // إنشاء كائن Room جديد
                Room rooms = new Room()
                {
                    Number = room.Number,
                    FloorNumber = room.FloorNumber,
                    HotelId = room.HotelId,
                    status = (Models.Status)room.status
                };
                // إضافة الغرفة إلى قاعدة البيانات
                roomRepository.Add(rooms);
                return CreatedAtAction("GetRoom", new { roomId = rooms.Id }, rooms);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء غير متوقع
                this.logger.LogCritical("حدث استثناء غير متوقع: خطأ في البرمجة", ex);
                return StatusCode(500, "حدث خطأ ما، يرجى المحاولة مرة أخرى لاحقًا");
            }
        }

        [AllowAnonymous]
        [HttpGet("{roomId}", Name = "GetRoom")]
        public async Task<ActionResult<Room>> GetRoom(int roomId)
        {
            try
            {
                // الحصول على معلومات الغرفة باستخدام رقم الهوية
                var room = await roomRepository.Get(roomId);
                if (room == null || room.IsDelete == true)
                {
                    logger.LogWarning($"هذه الغرفة برقم {roomId} غير موجودة. يرجى المحاولة مرة أخرى لاحقًا");
                    return NotFound();
                }
                return Ok(room);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء غير متوقع
                this.logger.LogCritical("حدث استثناء غير متوقع: خطأ في البرمجة", ex);
                return StatusCode(500, "حدث خطأ ما، يرجى المحاولة مرة أخرى لاحقًا");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            try
            {
                // الحصول على قائمة بجميع الغرف المتاحة
                var rooms = await roomRepository.GetAll();
                if (rooms == null)
                {
                    logger.LogWarning($"لا توجد غرف متاحة حاليًا. يرجى المحاولة مرة أخرى لاحقًا");
                    return NotFound();
                }
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء غير متوقع
                this.logger.LogCritical("حدث استثناء غير متوقع: خطأ في البرمجة", ex);
                return StatusCode(500, "حدث خطأ ما، يرجى المحاولة مرة أخرى لاحقًا");
            }
        }

        [HttpPut("{roomId}")]
        public async Task<ActionResult<Room>> UpdateRoom(int roomId, RoomForUpdate room)
        {
            try
            {
                // تحديث بيانات الغرفة باستخدام رقم الهوية
                var Room = await roomRepository.Update(roomId);
                if (Room == null || Room.IsDelete == true)
                {
                    logger.LogWarning($"هذه الغرفة برقم {roomId} غير موجودة. يرجى المحاولة مرة أخرى لاحقًا");
                    return NotFound();
                }
                context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء غير متوقع
                this.logger.LogCritical("حدث استثناء غير متوقع: خطأ في البرمجة", ex);
                return StatusCode(500, "حدث خطأ ما، يرجى المحاولة مرة أخرى لاحقًا");
            }
        }

        [HttpDelete("{roomId}")]
        public ActionResult<Room> DeleteRoom(int roomId)
        {
            try
            {
                // حذف الغرفة باستخدام رقم الهوية
                var room = context.Rooms.FirstOrDefault(r => r.Id == roomId);
                if (room == null)
                {
                    logger.LogWarning($"هذه الغرفة برقم {roomId} غير موجودة. يرجى المحاولة مرة أخرى لاحقًا");
                    return NotFound();
                }
                if (room.IsDelete == true)
                {
                    return BadRequest("تم حذف هذه الغرفة بالفعل");
                }
                roomRepository.Delete(roomId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حالة حدوث استثناء غير متوقع
                this.logger.LogCritical("حدث استثناء غير متوقع: خطأ في البرمجة", ex);
                return StatusCode(500, "حدث خطأ ما، يرجى المحاولة مرة أخرى لاحقًا");
            }
        }

    }
}
