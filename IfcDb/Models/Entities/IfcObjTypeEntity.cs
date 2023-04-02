using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IfcDb.Models.Entities
{
    public class IfcObjTypeEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
