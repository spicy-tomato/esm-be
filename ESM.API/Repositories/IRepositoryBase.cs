using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ESM.API.Repositories;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public interface IRepositoryBase<T> where T : class
{
    T? GetById(Guid id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
    T? Create(T entity, bool saveChanges);
    void CreateRange(IEnumerable<T> entities);
    bool Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}