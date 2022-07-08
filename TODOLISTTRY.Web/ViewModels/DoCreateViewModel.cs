using System;
using System.ComponentModel.DataAnnotations;
using TODOLISTTRY.Services.DTO;
using TODOLISTTRY.Web.Infrastructure.CustomValidationAttributes;

namespace TODOLISTTRY.Web.ViewModels
{
    public class DoCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "TitleLength")]
        [Display(Name = "TitleName")]
        public string Title { get; set; }

        [StringLength(200, MinimumLength = 1,
            ErrorMessage = "DescriptionLength")]
        [Display(Name = "DescriptionName")]
        public string Description { get; set; }

        [Display(Name = "ExecutorsName")]
        public string Executors { get; set; }

        [Required(ErrorMessage = "PlanRequired")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        [CustomDateTimeCheck("07/07/2022", "12/12/2100", ErrorMessage = "PlanDateTimeError")]
        [Display(Name = "PlanName")]
        public DateTime Plan { get; set; }

        public DoCreateViewModel(DoDTO model)
        {
            Id = model.Id;
            Title = model.Title;
        }

        public DoCreateViewModel()
        {

        }
    }

   
}
