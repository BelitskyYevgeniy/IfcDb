using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IfcDb.Models.Entities
{
    public class IfcObjDestinationEntity
    {
        [Key]
        public IfcObjDestination Id { get; set; }

        [Required]
        public string Destination { get; set; } = string.Empty;
    }
}
