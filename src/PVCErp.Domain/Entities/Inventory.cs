using PVCErp.Domain.Common;

namespace PVCErp.Domain.Entities;

public sealed class InventoryBatch : BaseEntity
{
    public Guid RawMaterialId { get; set; }
    public RawMaterial? RawMaterial { get; set; }
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public decimal ReceivedQuantityKg { get; set; }
    public decimal AvailableQuantityKg { get; set; }
    public QualityStatus QualityStatus { get; set; } = QualityStatus.Pending;
}

public sealed class GoodsReceiptNote : BaseEntity
{
    public string GrnNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public DateOnly ReceiptDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public QualityStatus QualityStatus { get; set; } = QualityStatus.Pending;
    public List<GoodsReceiptItem> Items { get; set; } = [];
}

public sealed class GoodsReceiptItem : BaseEntity
{
    public Guid GoodsReceiptNoteId { get; set; }
    public GoodsReceiptNote? GoodsReceiptNote { get; set; }
    public Guid RawMaterialId { get; set; }
    public RawMaterial? RawMaterial { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public decimal QuantityKg { get; set; }
}
