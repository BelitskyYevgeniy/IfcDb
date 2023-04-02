using System.Collections.Generic;
using System.Linq;

using IfcDb.Interfaces;
using IfcDb.Models;
using IfcDb.Models.Entities;

namespace IfcDb.Mappers
{
    public class IfcMapper : IIfcMapper
    {
        private readonly IIfcParser _parser;

        public IfcMapper(IIfcParser parser)
        {
            this._parser = parser;
        }

        public IfcAttributeEntity ToEntity(IfcAttribute model)
        {
            IfcAttributeEntity result = new IfcAttributeEntity();
            result.TypeId = model.Type;
            result.Value = new IfcValueEntity
            {
                Value = model.ToString()
            };
            if (model.Type == IfcAttributeType.Obj)
            {
                result.Obj = ToEntity((IfcObj)model.Value);
            };
            return result;
        }

        public IfcObjEntity ToEntity(IfcObj model)
        {
            IfcObjEntity result = new IfcObjEntity
            {
                No = model.Id,
                Type = new IfcObjTypeEntity
                {
                    Name = model.Name
                },
                DestinationId = model.Destination,
                Attributes = model.Attributes.Select(e => ToEntity(e)).ToList()
            };
            return result;
        }

        public IfcFileEntity ToEntity(IfcFile model)
        {
            IfcFileEntity result = new IfcFileEntity
            {
                Head = new List<IfcObjEntity>(model.Head.Count),
                Data = new List<IfcObjEntity>(model.Data.Count)
            };
            foreach (IfcObj obj in model.Head)
            {
                var entity = ToEntity(obj);
                entity.DestinationId = IfcObjDestination.Header;
                result.Head.Add(entity);
            }
            foreach (IfcObj obj in model.Data)
            {
                var entity = ToEntity(obj);
                entity.DestinationId = IfcObjDestination.Data;
                result.Data.Add(entity);
            }
            return result;
        }

        public IfcAttribute ToModel(IfcAttributeEntity entity)
        {
            if (entity.Value == null)
            {
                return null;
            }
            return _parser.ParseAttribute(entity.Value.Value);
        }

        public IfcObj ToModel(IfcObjEntity entity)
        {
            if (entity.Type == null)
            {
                return null;
            }
            IfcObj result = new IfcObj
            {
                Id = entity.No,
                Name = entity.Type.Name,
                Destination = entity.DestinationId,
                Attributes = entity.Attributes.Select(e => ToModel(e)).ToList()
            };
            return result;
        }

        public IfcFile ToModel(IfcFileEntity entity)
        {
            IfcFile result = new IfcFile
            {
                Head = entity.Head.Select(e => ToModel(e)).ToList(),
                Data = entity.Data.Select(e => ToModel(e)).ToList()
            };
            return result;
        }
    }
}
