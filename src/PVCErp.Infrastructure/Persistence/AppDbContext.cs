using Microsoft.EntityFrameworkCore;
using PVCErp.Domain.Common;
using PVCErp.Domain.Entities;

namespace PVCErp.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SocketingEntry> Socketings => Set<SocketingEntry>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<RawMaterial> RawMaterials => Set<RawMaterial>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<ScrewBarrel> ScrewBarrels => Set<ScrewBarrel>();
    public DbSet<InventoryBatch> InventoryBatches => Set<InventoryBatch>();
    public DbSet<GoodsReceiptNote> GoodsReceiptNotes => Set<GoodsReceiptNote>();
    public DbSet<GoodsReceiptItem> GoodsReceiptItems => Set<GoodsReceiptItem>();
    public DbSet<BatchFormula> BatchFormulas => Set<BatchFormula>();
    public DbSet<BatchFormulaItem> BatchFormulaItems => Set<BatchFormulaItem>();
    public DbSet<ProductionBatch> ProductionBatches => Set<ProductionBatch>();
    public DbSet<ProductionConsumption> ProductionConsumptions => Set<ProductionConsumption>();
    public DbSet<ProductionOutput> ProductionOutputs => Set<ProductionOutput>();
    public DbSet<ScrapRecord> ScrapRecords => Set<ScrapRecord>();
    public DbSet<DispatchChallan> DispatchChallans => Set<DispatchChallan>();
    public DbSet<DispatchItem> DispatchItems => Set<DispatchItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(entry => entry.State == EntityState.Modified))
        {
            entry.Entity.UpdatedAtUtc = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties().Where(property => property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(3);
            }
        }
    }
}
