using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TODOLISTTRY.Services.DTO;
using TODOLISTTRY.Web.ViewModels;

namespace TODOLISTTRY.Web.TagHelpers
{
    public static class HtmlTagHelpers
    {
        /// <summary>
        /// Выбор статусов для дроплиста в зависимости от текущего статуса
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="model">Модель для страницы обновления</param>
        /// <returns>Коллекцию доступных статусов с выключенным текущим</returns>
        public static IEnumerable<SelectListItem> StatusItemsUpdateViewModel(
            this IHtmlHelper htmlHelper,
            DoUpdateViewModel model)
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();

            switch(model.Status)
            {
                case DoDTOStatus.Created:
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Created.ToString()

                    });
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Processing.ToString()
                    });
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Paused.ToString()
                    });
                    break;

                case DoDTOStatus.Processing:
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Processing.ToString()
                    });
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Paused.ToString()
                    });
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Done.ToString()
                    });
                    break;

                case DoDTOStatus.Paused:
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Paused.ToString()
                    });
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Processing.ToString()
                        
                    });
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Done.ToString()
                    });
                    break;

                case DoDTOStatus.Done:
                    StatusList.Add(new SelectListItem
                    {
                        Text = DoDTOStatus.Done.ToString(),
                    });
                    break;

                default:
                    break;
            }

            return StatusList;
        }

    }
}
