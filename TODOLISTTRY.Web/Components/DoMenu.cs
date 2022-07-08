using Microsoft.AspNetCore.Mvc;
using TODOLISTTRY.Services.Interfaces;

namespace TODOLISTTRY.Web.Components
{
    public class DoMenu : ViewComponent
    {
        private readonly IDoService _modelService;

        public DoMenu(IDoService service)
        {
            _modelService = service;
        }

        public IViewComponentResult Invoke()
        {
            return View(_modelService.GetAllDoes());
        }
    }
}
