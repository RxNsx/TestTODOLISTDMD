using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TODOLISTTRY.DAL.Interfaces;
using TODOLISTTRY.DAL.Model;
using TODOLISTTRY.Services.DTO;
using TODOLISTTRY.Services.ExceptionHandlers;
using TODOLISTTRY.Services.Interfaces;

namespace TODOLISTTRY.Services.Services
{
    public class DoService : IDoService
    {
        private readonly IUnitOfWork _database;

        public DoService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        /// <summary>
        /// Получение всей коллекции задач
        /// </summary>
        /// <returns>Коллекция задач с подзадачами</returns>
        public IEnumerable<DoDTO> GetAllDoes()
        {
            var allDoDtoes = _database.Does
                .GetAllAsync().Result
                .Where(x =>
                    !_database.Does.GetAllAsync().Result
                    .Select(x => x.SubTasks)
                    .SelectMany(x => x)
                    .Contains(x))
                .Select(x => new DoDTO(x));

            return allDoDtoes;
        }

        /// <summary>
        /// Получение конкретной задачи по ID
        /// </summary>
        /// <param name="id">Ключ</param>
        /// <returns>Конкретная задача с нужным ключом</returns>
        public DoDTO GetByIdAsync(int id)
        {
            var doToDoDTO = _database.Does.GetByIdAsync(id).Result;

            if(doToDoDTO == null)
            {
                throw new DoNotFoundException("GetByIdAsync");
            }

            return new DoDTO(doToDoDTO);

        }

        /// <summary>
        /// Добавление новой задачи
        /// </summary>
        /// <param name="entity">Задача полученная через интерфейс</param>
        public async Task<int> CreateAsync(DoDTO model)
        {
            try
            {
                Do newDo = new Do
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    Created = DateTime.Now,
                    Status = DoStatus.Created,
                    Plan = model.Plan
                    
                };

                await _database.Does.CreateAsync(newDo);

                _database.Save();

                return newDo.Id;
            }
            catch (Exception ex)
            {
                throw new UnPredictableExceptions(ex.Message);
            }
        }

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="entity">Конкретная задача</param>
        public void Delete(int id)
        {
            try
            {
                var doToDelete = _database.Does.GetByIdAsync(id).Result;

                if (doToDelete != null)
                {
                        if (doToDelete.SubTasks.Any() && doToDelete.SubTasks != null)
                        {
                            ///Обнуляем список задач - обновляем
                            doToDelete.SubTasks = null;
                            _database.Does.Update(doToDelete);
                        }
                }

                _database.Does.Delete(doToDelete);

                _database.Save();
            }
            catch(Exception ex)
            {
                throw new UnPredictableExceptions(ex.Message);
            }
        }

        /// <summary>
        /// Обновление задачи
        /// </summary>
        /// <param name="entity">Задача полученная от пользователя</param>
        public void Update(DoDTO model)
        {
            var doToUpdate = _database.Does.GetByIdAsync(model.Id).Result;

            if(doToUpdate == null)
            {
                throw new DoNotFoundException("Update");
            }

            doToUpdate.Status = (DoStatus)model.Status;

            if (model.Status == DoDTOStatus.Done)
            {
                //Обновляем все задачи в статус выполненных если терминальная задача выполнена
                TerminalTaskDone(doToUpdate);
                //Устанавливаем время
                doToUpdate.Finished = DateTime.Now;
                doToUpdate.Fact = CalcFactTime(doToUpdate);
                //Проставляем фактическое время по всем подзадачам
                SetFactDateAllSubLists(doToUpdate);
            }
            else
            {
                doToUpdate.Status = (DoStatus)model.Status;
                doToUpdate.Fact = model.Fact;
            }

            doToUpdate.Title = model.Title;
            doToUpdate.Description = model.Description;
            doToUpdate.Executors = model.Executors;
            doToUpdate.Plan = CalcPlanTime(doToUpdate);

            ///Логика добавления в подзадачи
            if (model.SubTasks != null)
            {
                var allSubIds = model.SubTasks.Select(x => x.Id);
                var allSubTasks = _database.Does.AsQueryable()
                    .Where(x => allSubIds.Contains(x.Id));

                foreach (var item in allSubTasks)
                {
                    doToUpdate.SubTasks.Add(item);
                }
            }

            _database.Does.Update(doToUpdate);
            _database.Save();
        }

        /// <summary>
        /// Расставить фактическое время всем подзадачам
        /// </summary>
        public void SetFactDateAllSubLists(Do model)
        {
            foreach (var item in model.SubTasks)
            {
                item.Fact = CalcFactTime(item);
                item.Status = DoStatus.Done;

                if(item.SubTasks.Count > 0)
                {
                    SetFactDateAllSubLists(item);
                }
            }
        }

        /// <summary>
        /// Завершение терминальной задачи с завершением подзадач
        /// </summary>
        /// <param name="model">Модель</param>
        public void TerminalTaskDone(Do model)
        {
            if(IsSubTasksDoesToComplete(model))
            {
                foreach (var subtask in model.SubTasks)
                {
                    subtask.Status = DoStatus.Done;
                    subtask.Finished = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Проверка на возможность завершить задачу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsSubTasksDoesToComplete(Do model)
        {
            foreach(var subtask in model.SubTasks)
            {
                if(!(subtask.Status == DoStatus.Processing))
                {
                    throw new SetStatusException();
                }
            }

            return true;
        }

        /// <summary>
        /// Рассчёт суммарного времени планирования
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DateTime CalcPlanTime(Do model)
        {
            var timeSpan = GetAllPlanTime(model);

            return model.Plan + timeSpan;
        }

        /// <summary>
        /// Рассчёт планового времени во всех подзадачах
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        private TimeSpan GetAllPlanTime(Do model)
        {
            TimeSpan diff = TimeSpan.Zero;

            if(model.SubTasks.Count > 0)
            {
                foreach (var item in model.SubTasks)
                {
                    //Если Время в подзадаче дальше чем время основной задачи тогда считаем
                    if(item.Plan > model.Plan)
                    {
                        diff += item.Plan.Subtract(item.Created);
                    }

                    if(item.SubTasks.Count > 0)
                    {
                        diff.Add(GetAllPlanTime(item));
                    }
                }
            }

            return diff;
        }

        /// <summary>
        /// Рассчёт фактического времени во всех подзадачах
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DateTime? CalcFactTime(Do model)
        {
            var timeSpan = GetAllFactTime(model);

            if(model.Finished.HasValue)
            {
                model.Fact = model.Finished.Value.Add(timeSpan);

                return model.Fact;
            }
            else
            {
                model.Finished = model.Created;

                var result = model.Finished.Value.Add(timeSpan);

                result = result.Subtract(model.Finished.Value.TimeOfDay);

                return result;
            }
        }

        private TimeSpan GetAllFactTime(Do model)
        {
            TimeSpan diff = TimeSpan.Zero;  

            if (model.SubTasks.Count > 0)
            {
                foreach (var item in model.SubTasks)
                {
                    //Если Время в подзадаче дальше чем время основной задачи тогда считаем
                    if (item.Finished > model.Finished)
                    {
                        if(item.Finished.HasValue)
                        {
                            diff += item.Finished.Value.Subtract(item.Created);
                        }
                        else
                        {
                            diff += TimeSpan.Zero;
                        }
                    }

                    if (item.SubTasks.Count > 0)
                    {
                        diff.Add(GetAllFactTime(item));
                    }
                }
            }

            return diff;
        }
    }
}
