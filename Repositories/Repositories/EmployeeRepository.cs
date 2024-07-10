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
    public class EmployeeRepository<T> : IRepository<Employee>
    {
        private readonly HotelContext context;
        public EmployeeRepository(HotelContext context)
        {
            this.context = context;
        }
        public void Add(Employee employee)
        {
            context.Employee.Add(employee);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var employee = context.Employee.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                employee.IsDelete = true;
                context.SaveChanges();
            }
        }
        public async Task<Employee?> Get(int id)
        {
            return await context.Employee.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<List<Employee>> GetAll()
        {
            return await context.Employee.Where(e => e.IsDelete == false).ToListAsync();
        }
        public async Task<Employee?> Update(int id)
        {
            var employee = await context.Employee.FirstOrDefaultAsync(e => e.Id == id);

            return employee;
        }
    }
}
