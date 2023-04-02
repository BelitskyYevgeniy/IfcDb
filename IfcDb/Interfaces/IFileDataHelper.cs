using System.Threading;
using System.Threading.Tasks;

using IfcDb.Models;
using IfcDb.Models.Entities;

namespace IfcDb.Interfaces
{
    public interface IFileDataHelper
    {
        Task<IfcFileEntity> AddFileAsync(IfcFileEntity file, CancellationToken ct = default);

        Task<IfcFile> GetAsync(string filename, CancellationToken ct = default);

        IfcFileEntity ParseFile(string data);
    }
}
