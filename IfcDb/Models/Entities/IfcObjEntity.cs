using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IfcDb.Models.Entities
{
    public class IfcObjEntity
    {
        [Key]
        public int Id { get; set; }

        public int? No { get; set; }

        [Required]
        public int TypeId { get; set; }
        public virtual IfcObjTypeEntity Type { get; set; }

        [Required]
        public IfcObjDestination DestinationId { get; set; }
        public virtual IfcObjDestinationEntity Destination { get; set; }

        public virtual List<IfcFileEntity> Files { get; set; } = new List<IfcFileEntity>();

        public virtual List<IfcAttributeEntity> Attributes { get; set; } = new List<IfcAttributeEntity>();
    }
}
