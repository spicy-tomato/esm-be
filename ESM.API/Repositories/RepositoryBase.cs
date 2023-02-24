using System.Linq.Expressions;
using AutoMapper;
using ESM.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ESM.API.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected ApplicationContext Context { get; }
    protected readonly IMapper Mapper;

    protected RepositoryBase(ApplicationContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public T? GetById(Guid id) => Context.Set<T>().Find(id);

    public IEnumerable<T> GetAll() => Context.Set<T>().AsNoTracking();

    public T? FindOne(Expression<Func<T, bool>> expression) => Context.Set<T>().Where(expression).FirstOrDefault();

    public Task<T?> FindOneAsync(Expression<Func<T, bool>> expression) =>
        Context.Set<T>().Where(expression).FirstOrDefaultAsync();

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression) => Context.Set<T>().Where(expression);

    public Task<List<T>> FindAsync(Expression<Func<T, bool>> expression) =>
        Context.Set<T>().Where(expression).ToListAsync();

    public T Create(T entity, bool saveChanges = true)
    {
        var result = Context.Set<T>().Add(entity).Entity;
        if (saveChanges)
        {
            Context.SaveChanges();
        }

        return result;
    }

    public ValueTask<EntityEntry<T>> CreateAsync(T entity, bool saveChanges = true)
    {
        var result = Context.Set<T>().AddAsync(entity);
        if (saveChanges)
        {
            Context.SaveChangesAsync();
        }

        return result;
    }

    public void CreateRange(IEnumerable<T> entities) => Context.Set<T>().AddRange(entities);

    public Task CreateRangeAsync(IEnumerable<T> entities) => Context.Set<T>().AddRangeAsync(entities);

    public void Update(T entity) => Context.Set<T>().Update(entity);

    public void Delete(T entity) => Context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => Context.Set<T>().RemoveRange(entities);
}