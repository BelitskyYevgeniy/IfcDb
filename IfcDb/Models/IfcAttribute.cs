using System;
using System.Collections.Generic;
using System.Globalization;

namespace IfcDb.Models
{

    public class IfcAttribute
    {
        public IfcAttributeType Type;
        public object Value;

        public IfcAttribute() { }

        public IfcAttribute(IfcAttributeType type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case IfcAttributeType.Null:
                    return "$";
                case IfcAttributeType.Derive:
                    return "*";
                case IfcAttributeType.String:
                    return (string)Value;
                case IfcAttributeType.Real:
                    return Convert.ToString((double)Value, new CultureInfo("en-US"));
                case IfcAttributeType.Integer:
                    return $"{(int)Value}";
                case IfcAttributeType.EntityInstanceName: //!!!
                    return $"#{(int)Value}";
                case IfcAttributeType.Enumeration: //!!!
                    return $".{((string)Value).ToUpper()}.";
                case IfcAttributeType.List:
                    return $"({string.Join(",", ((List<IfcAttribute>)Value).ConvertAll(x => x.ToString()))})";
                case IfcAttributeType.Obj:
                    return ((IfcObj)Value).ToString();
                default: return (string)Value;
            }
        }
    }

}
