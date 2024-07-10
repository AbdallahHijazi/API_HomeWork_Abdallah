using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);

        Task<T> Get(int id);

        Task<List<T>> GetAll();

        void Delete(int id);

        Task<T> Update(int id);
    }
}
