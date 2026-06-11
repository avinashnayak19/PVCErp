using PVCErp.Domain.Common;

namespace PVCErp.Domain.Entities;

public sealed class ProductionOutput : BaseEntity
{
    public Guid ProductionBatchId { get; set; }
    public ProductionBatch? ProductionBatch { get; set; }
    public Guid MachineId { get; set; }
    public Machine? Machine { get; set; }
    public Guid ScrewBarrelId { get; set; }
    public ScrewBarrel? ScrewBarrel { get; set; }
    public ProductionStage Stage { get; set; } = ProductionStage.PipeProduction;
    public string PipeDimension { get; set; } = string.Empty;
    public decimal ApprovedQuantityKg { get; set; }
    public decimal RejectedQuantityKg { get; set; }
    public decimal HoursRun { get; set; }
}

public sealed class ScrapRecord : BaseEntity
{
    public ProductionStage Stage { get; set; }
    public string ScrapType { get; set; } = string.Empty;
    public decimal GeneratedKg { get; set; }
    public decimal ReusedKg { get; set; }
    public string? SourceReference { get; set; }
    public DateOnly RecordDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}
