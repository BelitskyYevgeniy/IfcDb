using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IfcDb.Models.Entities
{
    public class IfcAttributeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public IfcAttributeType TypeId { get; set; }
        public virtual IfcAttributeTypeEntity Type { get; set; }

        [Required]
        public int ValueId { get; set; }
        public virtual IfcValueEntity Value { get; set; }

        public int? ObjId { get; set; }
        public virtual IfcObjEntity? Obj { get; set; }
    }
}
