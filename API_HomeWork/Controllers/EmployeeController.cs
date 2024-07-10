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
    public class EmployeeController : ControllerBase
    {
        private readonly HotelContext context;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<Hotel> hotelRepository;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(HotelContext context
                                  , IRepository<Employee> employeeRepository
                                  , IRepository<Hotel> hotelRepository
                                  , ILogger<EmployeeController> logger)
        {
            this.context = context;
            this.employeeRepository = employeeRepository;
            this.hotelRepository = hotelRepository;
            this.logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(EmployeeForCreate employee)
        {
            try
            {
                // إنشاء كائن جديد من الموظف وإسناد القيم له
                Employee employees = new Employee()
                {
                    FirstName = employee.FirstName,
                    Lastname = employee.Lastname,
                    Title = employee.Title,
                    DOB = employee.DOB,
                    Email = employee.Email,
                    StartDate = employee.StartDate,
                    HotelId = employee.HotelId,
                };

                // التحقق من أن البريد الإلكتروني ليس فارغًا
                if (string.IsNullOrEmpty(employee.Email))
                {
                    return BadRequest();
                }

                // إضافة الموظف الجديد إلى المستودع
                employeeRepository.Add(employees);

                // إرجاع نتيجة الإنشاء بنجاح
                return CreatedAtAction("GetEmployee", new { employeeId = employees.Id }, employees);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إرجاع رمز حالة 500 مع رسالة خطأ
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        [AllowAnonymous]
        [HttpGet("{employeeId}", Name = "GetEmployee")]
        public async Task<ActionResult<Employee>> GetEmployee(int employeeId)
        {
            try
            {
                // جلب الموظف حسب المعرف
                var employee = await employeeRepository.Get(employeeId);

                // التحقق من وجود الموظف وأنه لم يتم حذفه
                if (employee == null || employee.IsDelete == true)
                {
                    logger.LogWarning($"This employee with ID {employeeId} does not exist. Please try again later");
                    return NotFound();
                }

                // إرجاع الموظف
                return Ok(employee);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إرجاع رمز حالة 500 مع رسالة خطأ
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            try
            {
                // جلب جميع الموظفين
                var employees = await employeeRepository.GetAll();

                // التحقق من وجود الموظفين
                if (employees == null)
                {
                    logger.LogWarning($"This employees does not exist. Please try again later");
                    return NotFound();
                }

                // إرجاع قائمة الموظفين
                return Ok(employees);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إرجاع رمز حالة 500 مع رسالة خطأ
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        [HttpPut("{employeeId}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int employeeId, EmployeeForUpdate employee)
        {
            try
            {
                // تحديث بيانات الموظف
                var Employee = await hotelRepository.Update(employeeId);

                // التحقق من وجود الموظف وأنه لم يتم حذفه
                if (Employee == null || Employee.IsDelete == true)
                {
                    logger.LogWarning($"This employee with ID {employeeId} does not exist. Please try again later");
                    return NotFound();
                }

                // تحديث معلومات الموظف
                employee.FirstName = employee.FirstName;
                employee.Lastname = employee.Lastname;
                employee.Title = employee.Title;
                employee.Email = employee.Email;
                employee.DOB = employee.DOB;

                // حفظ التغييرات في قاعدة البيانات
                employeeRepository.Get(employeeId);
                context.SaveChanges();

                // إرجاع نتيجة النجاح بدون محتوى
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إرجاع رمز حالة 500 مع رسالة خطأ
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
        [HttpDelete("{employeeId}")]
        public ActionResult<Employee> DeleteEmployee(int employeeId)
        {
            try
            {
                // جلب الموظف حسب المعرف
                var employee = context.Employee.FirstOrDefault(e => e.Id == employeeId);

                // التحقق من وجود الموظف
                if (employee == null)
                {
                    logger.LogWarning($"This employee with ID {employeeId} does not exist. Please try again later");
                    return NotFound();
                }

                // التحقق من أن الموظف لم يتم حذفه سابقًا
                if (employee.IsDelete == true)
                {
                    return BadRequest("This employee has already been deleted");
                }

                // حذف الموظف
                employeeRepository.Delete(employeeId);

                // إرجاع نتيجة النجاح بدون محتوى
                return NoContent();
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في السجل
                this.logger.LogCritical("Unhandled exception : erorr happend", ex);

                // إرجاع رمز حالة 500 مع رسالة خطأ
                return StatusCode(500, "An error occurred, please try again later");
            }
        }
    }
}
