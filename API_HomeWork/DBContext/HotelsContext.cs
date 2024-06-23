using Microsoft.EntityFrameworkCore;
using API_HomeWork.Models;

namespace API_HomeWork.DBContext
{
    public class HotelsContext : DbContext
    {
        public HotelsContext(DbContextOptions<HotelsContext> options) : base(options) 
        { 
        }
        public DbSet<Hotels> Hotels { get; set; }
        public DbSet<Guests> Guests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Stuff> Stuff { get; set; }
    }
}
    