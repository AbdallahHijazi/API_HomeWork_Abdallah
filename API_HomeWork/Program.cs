using API_HomeWork.Models;
using HotelDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Repositories;
using System.Text;
using Serilog;



namespace API_HomeWork
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true ;
            }).AddXmlDataContractSerializerFormatters();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Log.Logger = new LoggerConfiguration()
                              .MinimumLevel.Debug()
                              .WriteTo.Console()
                              .WriteTo.File("C:\\Users\\Abdal\\source\\repos\\API_HomeWork_Abdallah\\API_HomeWork", rollingInterval: RollingInterval.Day)
                              .CreateLogger();
            builder.Host.UseSerilog();


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddDbContext<HotelContext>(
                   options => options.UseSqlServer(builder.Configuration.GetConnectionString("HotelDBConnectionString")));
           
            builder.Services.AddScoped<IRepository<Hotel>, HotelRepository<Hotel>>()
                            .AddScoped<IRepository<Employee>, EmployeeRepository<Employee>>()
                            .AddScoped<IRepository<Guest>, GuestRepository<Guest>>()
                            .AddScoped<IRepository<Booking>, BookingRepository<Booking>>()
                            .AddScoped<IRepository<Room>, RoomRepository<Room>>();

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"])),
                    ValidateIssuer=true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey =true,

                };
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
