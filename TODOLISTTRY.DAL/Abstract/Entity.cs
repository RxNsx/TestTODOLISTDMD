using System.ComponentModel.DataAnnotations;
using TODOLISTTRY.DAL.Interfaces;

namespace TODOLISTTRY.DAL.Abstract
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
