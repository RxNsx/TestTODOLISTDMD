
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TODOLISTTRY.Services.DTO;
using TODOLISTTRY.Web.Infrastructure.CustomValidationAttributes;

namespace TODOLISTTRY.Web.ViewModels
{
    /// <summary>
    /// Модель обновления
    /// </summary>
    public class DoUpdateViewModel
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

        [Display(Name = "StatusName")]
        public DoDTOStatus Status { get; set; } = DoDTOStatus.Created;

        [Required(ErrorMessage = "PlanRequired")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        [CustomDateTimeCheck("07/07/2022", "12/12/2100", ErrorMessage = "PlanDateTimeError")]
        [Display(Name = "PlanName")]
        public DateTime Plan { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        [Display(Name = "FactName")]
        public DateTime? Fact { get; set; }

        [Display(Name = "SubtasksName")]
        public List<DoDTO> Subtasks { get; set; }
        public DoUpdateViewModel(DoDTO model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            Executors = model.Executors;
            Status = model.Status;
            Subtasks = model.SubTasks;
            Plan = model.Plan;
            Fact = model.Fact;
        }

        public DoUpdateViewModel()
        {

        }

        
    }
}
