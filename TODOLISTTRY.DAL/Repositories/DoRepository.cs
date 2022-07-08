using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TODOLISTTRY.DAL.Interfaces;
using TODOLISTTRY.DAL.Model;

namespace TODOLISTTRY.DAL.Repositories
{
    public class DoRepository : IRepository<Do>
    {
        private ApplicationDbContext _context;

        public DoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Найти по айдишнику асинхронно
        /// </summary>
        /// <param name="id">Айдишник</param>
        /// <returns>Экземпляр по айдишнику</returns>
        public async Task<Do> GetByIdAsync(int id)
        {
            return await _context.Set<Do>().AsQueryable().Include(s => s.SubTasks).FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Добавление сущности в коллекцию асинхронно
        /// </summary>
        /// <param name="entity"></param>
        public async Task<Do> CreateAsync(Do entity)
        {
            var x = await _context.Set<Do>().AddAsync(entity);

            return x.Entity;
        }

        /// <summary>
        /// Обновление задачи
        /// </summary>
        /// <param name="entity">Обновленный статус задачи</param>
        public Do Update(Do entity)
        {
            _context.Set<Do>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Удаление задачи из БД
        /// </summary>
        /// <param name="entity">Конкретная задача для удаления</param>
        public void Delete(Do entity)
        {
            _context.Remove(entity);
        }

        /// <summary>
        /// Получить всю коллекцию асинхронно
        /// </summary>
        /// <returns>Коллекция данных</returns>
        public async Task<IEnumerable<Do>> GetAllAsync()
        {
            return await _context.Set<Do>().Include(s => s.SubTasks).ToListAsync();
        }

        /// <summary>
        /// Получение коллекцию всех элементов напрямую из БД
        /// </summary>
        /// <returns></returns>
        public IQueryable<Do> AsQueryable()
        {
            return _context.Set<Do>().AsQueryable();
        }

    }
}
