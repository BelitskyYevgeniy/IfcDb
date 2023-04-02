using System.Collections.Generic;

namespace IfcDb.Models
{

    public class IfcObj
    {
        public int? Id;
        public string Name;
        public IfcObjDestination Destination;
        public List<IfcAttribute> Attributes;

        public override string ToString()
        {
            return Id > 0 ? $"#{Id} = {Name}({string.Join(",", Attributes)});" : $"{Name}({string.Join(",", Attributes)})";
        }
    }

}