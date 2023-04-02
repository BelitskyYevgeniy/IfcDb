using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfcDb.Models.Entities
{
    public class IfcFileEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual List<IfcObjEntity> Objects { get; set; } = new List<IfcObjEntity>();

        [NotMapped]
        public List<IfcObjEntity> Head { get; set; } = new List<IfcObjEntity>();
        [NotMapped]
        public List<IfcObjEntity> Data { get; set; } = new List<IfcObjEntity>();
    }
}
