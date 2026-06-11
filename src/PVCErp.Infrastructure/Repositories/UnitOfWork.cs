using PVCErp.Application.Abstractions;
using PVCErp.Infrastructure.Persistence;

namespace PVCErp.Infrastructure.Repositories;

public sealed class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
