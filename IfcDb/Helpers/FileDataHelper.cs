using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using IfcDb.Interfaces;
using IfcDb.Models;
using IfcDb.Models.Entities;

namespace IfcDb.Helpers
{
    public class FileDataHelper : IFileDataHelper
    {
        private readonly IfcDbContext _dbContext;
        private readonly IIfcMapper _ifcMapper;
        private readonly IIfcParser _parser;

        public FileDataHelper(IfcDbContext dbContext, IIfcMapper ifcMapper, IIfcParser parser)
        {
            _dbContext = dbContext;
            _ifcMapper = ifcMapper;
            _parser = parser;
        }

        private async Task<IEnumerable<IfcObjEntity>> addObjectsAsync(IEnumerable<IfcObjEntity> objects, CancellationToken ct = default)
        {
            var result = new List<IfcObjEntity>();
            foreach (var obj in objects)
            {
                var entity = await addObjAsync(obj, ct);
                result.Add(entity);
            }
            return result;
        }

        private async Task<IfcAttributeEntity> addValueAsync(IfcAttributeEntity attribute, CancellationToken ct = default)
        {
            if (attribute.Obj != null)
            {
                attribute.Obj = await addObjAsync(attribute.Obj, ct);
            }

            var value = await _dbContext.Values.FirstOrDefaultAsync(e => e.Value == attribute.Value.Value, ct);
            if (value == null)
            {
                attribute.Value = (await _dbContext.Values.AddAsync(new IfcValueEntity { Value = attribute.Value.Value }, ct)).Entity;
            }
            var result = (await _dbContext.AddAsync(attribute, ct)).Entity;
            await _dbContext.SaveChangesAsync(ct);

            return result;
        }

        private async Task<IfcObjEntity> addObjAsync(IfcObjEntity obj, CancellationToken ct = default)
        {
            var attributeEntities = new List<IfcAttributeEntity>(obj.Attributes.Count);
            foreach (var attribute in obj.Attributes)
            {
                var entity = await addValueAsync(attribute, ct);
                attributeEntities.Add(entity);
            }

            var objTypeEntity = await _dbContext.ObjectTypes.FirstOrDefaultAsync(e => e.Name == obj.Type.Name, ct);
            if (objTypeEntity == null)
            {
                obj.Type = (await _dbContext.ObjectTypes.AddAsync(new IfcObjTypeEntity { Name = obj.Type.Name }, ct)).Entity;
            }
            obj.Attributes = attributeEntities;
            var result = (await _dbContext.AddAsync(obj, ct)).Entity;
            await _dbContext.SaveChangesAsync(ct);

            return result;
        }

        public async Task<IfcFileEntity> AddFileAsync(IfcFileEntity file, CancellationToken ct = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

            var headObjects = await addObjectsAsync(file.Head);
            var dataObjects = await addObjectsAsync(file.Data);

            file.Objects = headObjects.ToList();
            file.Objects.AddRange(dataObjects);

            var result = (await _dbContext.AddAsync(file, ct)).Entity;

            await _dbContext.SaveChangesAsync(ct);

            transaction.Commit();

            return result;
        }

        public async Task<IfcFile> GetAsync(string filename, CancellationToken ct = default)
        {
            var file = await _dbContext.Files.FirstOrDefaultAsync(f => f.Name == filename, ct);
            if (file == null)
            {
                return null;
            }
            var objectsQuery = _dbContext.Entry(file).Collection(e => e.Objects).Query();
            file.Head = await objectsQuery.Where(e => e.DestinationId == IfcObjDestination.Header).OrderBy(e => e.Id).ToListAsync(ct);
            file.Data = await objectsQuery.Where(e => e.DestinationId == IfcObjDestination.Data && e.No.HasValue).OrderBy(e => e.Id).ToListAsync(ct);
            return _ifcMapper.ToModel(file);
        }

        public IfcFileEntity ParseFile(string data)
        {
            var ifcFile = _parser.ParseFile(data);
            return _ifcMapper.ToEntity(ifcFile);
        }
    }
}
