using PVCErp.Domain.Common;

namespace PVCErp.Application.Dtos;

public sealed record FormulaItemRequest(Guid RawMaterialId, decimal StandardQuantityKg);
public sealed record CreateBatchFormulaRequest(string FormulaCode, string Description, List<FormulaItemRequest> Items);
public sealed record BatchFormulaDto(Guid Id, string FormulaCode, string Description, int MaterialCount);
public sealed record CreateMachineRequest(string MachineNumber, string? Description);
public sealed record MachineDto(Guid Id, string MachineNumber, string? Description);
public sealed record CreateScrewBarrelRequest(string BarrelNumber, string? Type, decimal TargetKgPerHour);
public sealed record ScrewBarrelDto(Guid Id, string BarrelNumber, string? Type, decimal TargetKgPerHour);
public sealed record ConsumptionRequest(Guid RawMaterialId, decimal StandardQuantityKg, decimal ActualQuantityKg);
public sealed record OutputRequest(Guid MachineId, Guid ScrewBarrelId, ProductionStage Stage, string PipeDimension, decimal ApprovedQuantityKg, decimal RejectedQuantityKg, decimal HoursRun);
public sealed record CreateProductionBatchRequest(Guid BatchFormulaId, DateOnly ProductionDate, string Shift, string OperatorName, string? SupervisorName, List<ConsumptionRequest> Consumptions, List<OutputRequest> Outputs);
public sealed record ProductionBatchDto(Guid Id, string BatchNumber, DateOnly ProductionDate, string Shift, string OperatorName, decimal ApprovedKg, decimal RejectedKg);
public sealed record ScrapRequest(ProductionStage Stage, string ScrapType, decimal GeneratedKg, decimal ReusedKg, string? SourceReference, DateOnly RecordDate);
public sealed record ScrapDto(Guid Id, ProductionStage Stage, string ScrapType, decimal GeneratedKg, decimal ReusedKg, decimal RecoveryPercent);
public sealed record EfficiencyDto(string BarrelNumber, decimal ProducedKg, decimal HoursRun, decimal KgPerHour, decimal TargetKgPerHour);
