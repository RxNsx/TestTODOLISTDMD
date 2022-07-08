using System.Collections.Generic;
using System.Threading.Tasks;
using TODOLISTTRY.Services.DTO;

namespace TODOLISTTRY.Services.Interfaces
{
    public interface IDoService
    {
        IEnumerable<DoDTO> GetAllDoes();
        DoDTO GetByIdAsync(int id);
        Task<int> CreateAsync(DoDTO model);
        void Delete(int id);
        void Update(DoDTO model);
    }
}
