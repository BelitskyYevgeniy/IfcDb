using System.ComponentModel.DataAnnotations;

namespace IfcDb.Models.Entities
{
    public class IfcValueEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
