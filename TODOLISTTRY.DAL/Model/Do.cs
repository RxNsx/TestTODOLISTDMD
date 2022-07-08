using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TODOLISTTRY.DAL.Abstract;

namespace TODOLISTTRY.DAL.Model
{
    /// <summary>
    /// Модель задачи
    /// </summary>
    public class Do : DoEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DoStatus Status { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public DateTime Plan { get; set; }
        public DateTime? Fact { get; set; }
        public DateTime? Finished { get; set; }
        public virtual List<Do> SubTasks { get; set; } = new List<Do>();
    }
}
