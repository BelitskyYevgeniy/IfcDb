using IfcDb.Models;
using IfcDb.Models.Entities;

namespace IfcDb.Interfaces
{
    public interface IIfcMapper : IMapper<IfcAttributeEntity, IfcAttribute>, IMapper<IfcObjEntity, IfcObj>, IMapper<IfcFileEntity, IfcFile>
    {
    }
}
