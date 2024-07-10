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
    public class GuestRepository<T> : IRepository<Guest>
    {
        private readonly HotelContext context;
        public GuestRepository(HotelContext context)
        {
            this.context = context;
        }
        public void Add(Guest guest)
        {
            context.Guests.Add(guest);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var guest = context.Guests.FirstOrDefault(g => g.Id == id);

            if (guest != null)
            {
                guest.IsDelete = true;
                context.SaveChanges();
            }
        }
        public async Task<Guest?> Get(int id)
        {
            return await context.Guests.FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task<List<Guest>> GetAll()
        {
            return await context.Guests.Where(g => g.IsDelete == false).ToListAsync();
        }
        public async Task<Guest?> Update(int id)
        {
            var guest = await context.Guests.FirstOrDefaultAsync(g => g.Id == id);

            return guest;
        }
    }
}
