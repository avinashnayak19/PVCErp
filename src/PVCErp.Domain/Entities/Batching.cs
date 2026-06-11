using PVCErp.Domain.Common;

namespace PVCErp.Domain.Entities;

public sealed class BatchFormula : BaseEntity
{
    public string FormulaCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public List<BatchFormulaItem> Items { get; set; } = [];
}

public sealed class BatchFormulaItem : BaseEntity
{
    public Guid BatchFormulaId { get; set; }
    public BatchFormula? BatchFormula { get; set; }
    public Guid RawMaterialId { get; set; }
    public RawMaterial? RawMaterial { get; set; }
    public decimal StandardQuantityKg { get; set; }
}

public sealed class ProductionBatch : BaseEntity
{
    public string BatchNumber { get; set; } = string.Empty;
    public Guid BatchFormulaId { get; set; }
    public BatchFormula? BatchFormula { get; set; }
    public DateOnly ProductionDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string Shift { get; set; } = "A";
    public string OperatorName { get; set; } = string.Empty;
    public string? SupervisorName { get; set; }
    public bool SupervisorApproved { get; set; }
    public List<ProductionConsumption> Consumptions { get; set; } = [];
    public List<ProductionOutput> Outputs { get; set; } = [];
}

public sealed class ProductionConsumption : BaseEntity
{
    public Guid ProductionBatchId { get; set; }
    public ProductionBatch? ProductionBatch { get; set; }
    public Guid RawMaterialId { get; set; }
    public RawMaterial? RawMaterial { get; set; }
    public decimal StandardQuantityKg { get; set; }
    public decimal ActualQuantityKg { get; set; }
}
