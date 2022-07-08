using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TODOLISTTRY.Services.DTO;
using TODOLISTTRY.Services.ExceptionHandlers;
using TODOLISTTRY.Services.Interfaces;
using TODOLISTTRY.Web.ViewModels;

namespace TODOLISTTRY.Web.Controllers
{
    public class DoController : Controller
    {
        private readonly ILogger<DoController> _logger;
        private readonly IDoService _modelService;

        public DoController(ILogger<DoController> logger,IDoService modelService)
        {
            _logger = logger;
            _modelService = modelService;
        }

        /// <summary>
        /// Главная страница
        /// </summary>
        /// <returns>Главная страница</returns>
        [HttpGet]
        public IActionResult Index()
        {
            var does = _modelService.GetAllDoes();

            return View(does);
        }
        
        [HttpGet]
        /// <summary>
        /// Страница добавления подзадачи
        /// </summary>
        /// <returns></returns>
        public IActionResult AddSubTaskView(int id)
        {
            var doToAddSubtask = _modelService.GetByIdAsync(id);

            if (doToAddSubtask == null)
            {
                _logger.LogError("Модель для добавления подзадачи не найдена");
                return View("Index");
            }

            ViewBag.TaskTitle = doToAddSubtask.Title;

            return View(new DoCreateSubTaskViewModel(doToAddSubtask));
        }

        /// <summary>
        /// Добавление подзадачи в терминальную задачу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddSubTask(int id,
            [Bind("Id,Title,Description,Executors,Plan")] DoCreateSubTaskViewModel model
            )
        {
            var terminal = _modelService.GetByIdAsync(id);

            if(terminal == null)
            {
                _logger.LogError("Невозможо получить терминальную задачу");
                throw new DoNotFoundException("AddSubTask {terminal}");
            }

            if (ModelState.IsValid)
            {
                var newSubTask = new DoDTO
                {
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    Plan = model.Plan
                };

                newSubTask = _modelService.GetByIdAsync(_modelService.CreateAsync(newSubTask).Result);

                terminal.SubTasks.Add(newSubTask);
                _modelService.Update(terminal);

                _logger.LogInformation("Терминальная задача была обновлена");

                return RedirectToAction("Index");
            }

            var doToAddSubtaskR = _modelService.GetByIdAsync(model.Id);

            return View("AddSubTaskView",new DoCreateSubTaskViewModel(doToAddSubtaskR));
        }


        /// <summary>
        /// Страница создания задачи
        /// </summary>
        /// <returns>Страница создания задачи</returns>
        public ViewResult CreateDoView()
        {
            return View(new DoCreateViewModel());
        }

        /// <summary>
        /// Отправка данных с формы для сощдания задачи
        /// </summary>
        /// <param name="model">Сформированная модель</param>
        /// <returns>Главная страница</returns>
        [HttpPost]
        public IActionResult CreateDo(
            [Bind("Title,Description,Executors,Plan")] DoCreateViewModel model
            )
        {
            if (ModelState.IsValid)
            {
                var newDTO = new DoDTO
                {
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    Plan = model.Plan
                };

                _modelService.CreateAsync(newDTO);

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogWarning("Созданная модель не прошла валидацию");
                return View("CreateDoView", new DoCreateViewModel());
            }
        }

        /// <summary>
        /// Страница обновления задачи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult UpdateView(int id)
        {
            var doDTOToFind = _modelService.GetByIdAsync(id);

            if(doDTOToFind == null)
            {
                _logger.LogError($"Не получилось найти задачу c Id: {doDTOToFind} в БД для обновления");
                throw new DoNotFoundException("AddSubTask");
            }

            return View(new DoUpdateViewModel(doDTOToFind));
        }

        /// <summary>
        /// Обновление модели
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Главную страницу</returns>
        [HttpPost]
        public IActionResult Update(
            [Bind("Id,Title,Description,Executors,Status,Plan,Fact")] DoUpdateViewModel model
            )
        {
            var dtoToUpdate = new DoDTO
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Executors = model.Executors,
                Status = model.Status,
                SubTasks = model.Subtasks,
                Plan = model.Plan,
                Fact = model.Fact
            };

            _modelService.Update(dtoToUpdate);

            _logger.LogInformation($"Модель с Id: {dtoToUpdate.Id} удачно обновилась!");;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="id">Айдишник задачи которую нужно удалить</param>
        /// <returns>Главную страницу</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            _modelService.Delete(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Страница информации о модели
        /// </summary>
        /// <param name="id">Конкретная модель</param>
        /// <returns>Представление информации о модели</returns>
        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            var doDetails = _modelService.GetByIdAsync(id);

            if (doDetails == null)
            {
                _logger.LogError($"Модель с Id {doDetails} не удалось получить в методе GetDetailsView");
                return View("Index");
            }

            return View(new DoDetailsViewModel(doDetails));
        }

        /// <summary>
        /// Переключение языка
        /// </summary>
        /// <param name="culture">Культура</param>
        /// <param name="returnUrl">Возвращаемый адрес</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(30)}
                );

            return LocalRedirect(returnUrl);
        }
    }
}
