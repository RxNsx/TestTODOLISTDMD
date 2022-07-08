using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOLISTTRY.DAL.Model;

namespace TODOLISTTRY.Services.DTO
{
    public class DoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DoDTOStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Plan { get; set; }
        public DateTime? Fact { get; set; }
        public DateTime? Finished { get; set; }
        public List<DoDTO> SubTasks { get; set; } = new List<DoDTO>();

        public DoDTO(Do entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Executors = entity.Executors;
            Created = entity.Created;
            Status = (DoDTOStatus)entity.Status;
            Plan = entity.Plan;
            Fact = entity.Fact;

            if (entity.SubTasks != null && entity.SubTasks.Any())
            {
                SubTasks = entity.SubTasks.Select(s => new DoDTO(s)).ToList();
            }
            else
            {
                SubTasks = new List<DoDTO>();
            }
        }

        public DoDTO()
        {

        }
    }


}
