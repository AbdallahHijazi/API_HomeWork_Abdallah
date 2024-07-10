using HotelsManegment.VewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_HomeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Accounts : ControllerBase
    {
        private readonly IConfiguration configuration;

        public Accounts(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthRequest request)
        {
            // تحقق من معلومات المستخدم باستخدام الطريقة ValidateUserInformation
            var user = ValidateUserInformation(request.userName, request.password);

            // إذا كان المستخدم غير موجود، قم بإرجاع حالة غير مصرح
            if (user == null)
            {
                return Unauthorized();
            }

            // إنشاء قائمة المطالبات الخاصة بالتوكن
            var Claims = new List<Claim>();
            Claims.Add(new Claim("course", "Midad-11")); // إضافة مطالبة للمساق

            // إعداد المفتاح السري لتوقيع التوكن
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecretKey"]));
            var SigningCred = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // إنشاء التوكن باستخدام JwtSecurityToken
            var securityToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"], // مصدر التوكن
                configuration["Authentication:Audiance"], // المستلم
                Claims, // المطالبات
                DateTime.UtcNow, // تاريخ الإصدار
                DateTime.UtcNow.AddHours(10), // تاريخ الانتهاء
                SigningCred // بيانات التوقيع
            );

            // كتابة التوكن وتحويله إلى نص
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            // تحقق من صحة اسم المستخدم وكلمة المرور مع القيم المخزنة في الإعدادات
            if (request.userName == configuration["Login:Username"] && request.password == configuration["Login:Password"])
                return Ok(token); // إذا كانت المعلومات صحيحة، أعد التوكن
            else
                return Unauthorized("The username or password is incorrect, please try again "); // إذا كانت المعلومات غير صحيحة، أعد رسالة خطأ
        }
        // تحقق من معلومات المستخدم (هذه الطريقة تحتاج إلى تحسين لتصبح فعالة)
        private object ValidateUserInformation(string userName, string password)
        {
            // إنشاء كائن مستخدم وهمي
            return new AmazonUser { FirstName = "Ahmad", LastName = "Hijazi", UserName = "mmmmmmm", UserId = 1 };
        }
    }
}
