using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCErp.Domain.Entities;

namespace PVCErp.Infrastructure.Persistence;

public sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.Property(item => item.Name).HasMaxLength(160).IsRequired();
        builder.Property(item => item.Country).HasMaxLength(80);
        builder.Property(item => item.GstNumber).HasMaxLength(32);
    }
}

public sealed class RawMaterialConfiguration : IEntityTypeConfiguration<RawMaterial>
{
    public void Configure(EntityTypeBuilder<RawMaterial> builder)
    {
        builder.Property(item => item.Name).HasMaxLength(120).IsRequired();
        builder.HasIndex(item => item.Name).IsUnique();
    }
}

public sealed class InventoryBatchConfiguration : IEntityTypeConfiguration<InventoryBatch>
{
    public void Configure(EntityTypeBuilder<InventoryBatch> builder)
    {
        builder.Property(item => item.BatchNumber).HasMaxLength(80).IsRequired();
        builder.HasOne(item => item.RawMaterial).WithMany().HasForeignKey(item => item.RawMaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(item => item.Supplier).WithMany().HasForeignKey(item => item.SupplierId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class GoodsReceiptNoteConfiguration : IEntityTypeConfiguration<GoodsReceiptNote>
{
    public void Configure(EntityTypeBuilder<GoodsReceiptNote> builder)
    {
        builder.Property(item => item.GrnNumber).HasMaxLength(40).IsRequired();
        builder.HasIndex(item => item.GrnNumber).IsUnique();
        builder.HasMany(item => item.Items).WithOne(item => item.GoodsReceiptNote).HasForeignKey(item => item.GoodsReceiptNoteId);
    }
}

public sealed class BatchFormulaConfiguration : IEntityTypeConfiguration<BatchFormula>
{
    public void Configure(EntityTypeBuilder<BatchFormula> builder)
    {
        builder.Property(item => item.FormulaCode).HasMaxLength(40).IsRequired();
        builder.HasIndex(item => item.FormulaCode).IsUnique();
        builder.HasMany(item => item.Items).WithOne(item => item.BatchFormula).HasForeignKey(item => item.BatchFormulaId);
    }
}

public sealed class ProductionBatchConfiguration : IEntityTypeConfiguration<ProductionBatch>
{
    public void Configure(EntityTypeBuilder<ProductionBatch> builder)
    {
        builder.Property(item => item.BatchNumber).HasMaxLength(40).IsRequired();
        builder.HasMany(item => item.Consumptions).WithOne(item => item.ProductionBatch).HasForeignKey(item => item.ProductionBatchId);
        builder.HasMany(item => item.Outputs).WithOne(item => item.ProductionBatch).HasForeignKey(item => item.ProductionBatchId);
    }
}

public sealed class DispatchConfiguration : IEntityTypeConfiguration<DispatchChallan>
{
    public void Configure(EntityTypeBuilder<DispatchChallan> builder)
    {
        builder.Property(item => item.ChallanNumber).HasMaxLength(40).IsRequired();
        builder.HasMany(item => item.Items).WithOne(item => item.DispatchChallan).HasForeignKey(item => item.DispatchChallanId);
    }
}

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.Property(item => item.InvoiceNumber).HasMaxLength(40).IsRequired();
        builder.HasIndex(item => item.InvoiceNumber).IsUnique();
    }
}
