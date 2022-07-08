using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOLISTTRY.DAL.Interfaces;
using TODOLISTTRY.DAL.Model;
using TODOLISTTRY.DAL.Repositories;

namespace TODOLISTTRY.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private IRepository<Do> _doReposiotry;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Do> Does
        {
            get
            {
                if (_doReposiotry == null)
                {
                    _doReposiotry = new DoRepository(_context);
                }
                return _doReposiotry;
            }
        }

        /// <summary>
        /// Cохранение изменений в БД
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
             _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
