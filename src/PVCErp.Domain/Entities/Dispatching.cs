using PVCErp.Domain.Common;

namespace PVCErp.Domain.Entities;

public sealed class DispatchChallan : BaseEntity
{
    public string ChallanNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public DateOnly DispatchDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DispatchStatus Status { get; set; } = DispatchStatus.Pending;
    public string? VehicleNumber { get; set; }
    public List<DispatchItem> Items { get; set; } = [];
}

public sealed class DispatchItem : BaseEntity
{
    public Guid DispatchChallanId { get; set; }
    public DispatchChallan? DispatchChallan { get; set; }
    public string PipeDimension { get; set; } = string.Empty;
    public decimal QuantityKg { get; set; }
    public int QuantityPieces { get; set; }
}

public sealed class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid DispatchChallanId { get; set; }
    public DispatchChallan? DispatchChallan { get; set; }
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public decimal TaxableAmount { get; set; }
    public decimal GstAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
}
