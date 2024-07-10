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
    public class HotelRepository<T> : IRepository<Hotel>
    {
            private readonly HotelContext context;
            public HotelRepository(HotelContext context)
            {
                      this.context = context;
            }
            public void Add(Hotel hotel)
            {
            context.Hotels.Add(hotel);
            context.SaveChanges();
            }
            public void Delete(int id)
            {
                var hotel = context.Hotels.FirstOrDefault(h => h.Id == id);

                if (hotel != null)
                {
                     hotel.IsDelete = true;
                    context.SaveChanges();
                }
            }
            public async Task<Hotel?> Get(int id)
            {
                return await context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            }
            public async Task<List<Hotel>> GetAll()
            {
                return await context.Hotels.Where(h => h.IsDelete == false).ToListAsync();
            }
            public async Task<Hotel?> Update(int id)
            {
                var hotel = await context.Hotels.FirstOrDefaultAsync(h => h.Id == id);

                return hotel;
            }
    }
}
