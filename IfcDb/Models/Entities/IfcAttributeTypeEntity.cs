using System.ComponentModel.DataAnnotations;

namespace IfcDb.Models.Entities
{
    public class IfcAttributeTypeEntity
    {
        [Key]
        public IfcAttributeType Id;
        [Required]
        public string Name { get; set; }
    }
}
