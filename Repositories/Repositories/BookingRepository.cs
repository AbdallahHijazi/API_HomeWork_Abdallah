using API_HomeWork.Models;
using HotelDataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BookingRepository<T> : IRepository<Booking>
    {
        private readonly HotelContext context;
        public BookingRepository(HotelContext context)
        {
            this.context = context;
        }
        public void Add(Booking booking)
        {
            context.Bookings.Add(booking);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var booking = context.Bookings.FirstOrDefault(b => b.Id == id);

            if (booking != null)
            {
                booking.IsDelete = true;
                context.SaveChanges();
            }
        }
        public async Task<Booking?> Get(int id)
        {
            return await context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<Booking>> GetAll()
        {
            return await context.Bookings.Where(b => b.IsDelete == false).ToListAsync();
        }
        public async Task<Booking?> Update(int id)
        {
            var booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            return booking;
        }
    }
}
