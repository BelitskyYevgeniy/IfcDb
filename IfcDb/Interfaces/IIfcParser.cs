using IfcDb.Models;

namespace IfcDb.Interfaces
{
    public interface IIfcParser
    {
        IfcAttribute ParseAttribute(string str);
        IfcObj ParseObj(string str);
        IfcFile ParseFile(string str);
    }
}
