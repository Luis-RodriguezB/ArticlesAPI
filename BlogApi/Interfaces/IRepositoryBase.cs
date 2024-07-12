namespace ArticlesAPI.Interfaces;
public interface IRepositoryBase<TEntity> : IDisposable
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(int id);
    Task<TEntity> Save(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(int id);
    Task<bool> Exist(int id);
}
