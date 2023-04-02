using System.Collections.Generic;

namespace IfcDb.Models
{

    public class IfcFile
    {
        public List<IfcObj> Head { get; set; } = new List<IfcObj>();
        public List<IfcObj> Data { get; set; } = new List<IfcObj>();

        public override string ToString()
        {
            return $"ISO-10303-21;\nHEADER;\n{string.Join("\n", Head)}\nENDSEC;\n" +
                   $"DATA;\n{string.Join("\n", Data)}\nENDSEC;\nEND-ISO-10303-21;";
        }
    }

}