namespace IfcDb.Interfaces
{
    public interface IMapper<TEntity, TIfcModel>
    {
        TEntity ToEntity(TIfcModel model);
        TIfcModel ToModel(TEntity entity);
    }
}
