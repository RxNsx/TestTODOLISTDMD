using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODOLISTTRY.DAL.Interfaces
{
    public interface IRepository<Do>
    {
        Task<Do> CreateAsync(Do entity);
        void Delete(Do entity);
        Do Update(Do entity);
        Task<Do> GetByIdAsync(int id);
        Task<IEnumerable<Do>> GetAllAsync();
        IQueryable<Do> AsQueryable();
    }
}
