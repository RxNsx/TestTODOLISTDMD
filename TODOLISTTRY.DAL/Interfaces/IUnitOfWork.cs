using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOLISTTRY.DAL.Model;

namespace TODOLISTTRY.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Do> Does { get; }
        void Save();
    }
}
