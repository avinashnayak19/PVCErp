using PVCErp.Domain.Common;

namespace PVCErp.Application.Dtos;

public sealed record RawMaterialDto(Guid Id, string Name, string Unit, decimal ReorderLevel, decimal AvailableKg);
public sealed record CreateRawMaterialRequest(string Name, string Unit, decimal ReorderLevel);
public sealed record CreateSupplierRequest(string Name, string? Country, string? GstNumber, string? Phone);
public sealed record SupplierDto(Guid Id, string Name, string? Country, string? GstNumber, string? Phone);
public sealed record GrnItemRequest(Guid RawMaterialId, string BatchNumber, decimal QuantityKg);
public sealed record CreateGrnRequest(Guid SupplierId, DateOnly ReceiptDate, QualityStatus QualityStatus, List<GrnItemRequest> Items);
public sealed record GrnDto(Guid Id, string GrnNumber, Guid SupplierId, DateOnly ReceiptDate, QualityStatus QualityStatus);
public sealed record GrnGetDto(Guid Id, string GrnNumber, Guid SupplierId, string SupplierName, DateOnly Date, string Material, string Weight, string QC);
