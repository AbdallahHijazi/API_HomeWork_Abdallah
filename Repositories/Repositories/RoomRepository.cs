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
    public class RoomRepository<T> : IRepository<Room>
    {
        private readonly HotelContext context;
        public RoomRepository(HotelContext context)
        {
            this.context = context;
        }
        public void Add(Room room)
        {
            context.Rooms.Add(room);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var room = context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room != null)
            {
                room.IsDelete = true;
                context.SaveChanges();
            }
        }
        public async Task<Room?> Get(int id)
        {
            return await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<List<Room>> GetAll()
        {
            return await context.Rooms.Where(r => r.IsDelete == false).ToListAsync();
        }
        public async Task<Room?> Update(int id)
        {
            var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);

            return room;
        }
    }
}
