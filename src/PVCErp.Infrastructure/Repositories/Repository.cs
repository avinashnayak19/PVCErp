using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PVCErp.Application.Abstractions;
using PVCErp.Domain.Common;
using PVCErp.Infrastructure.Persistence;

namespace PVCErp.Infrastructure.Repositories;

public sealed class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Set<T>().FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await dbContext.Set<T>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => dbContext.Set<T>().Update(entity);

    public void Remove(T entity) => dbContext.Set<T>().Remove(entity);
}
