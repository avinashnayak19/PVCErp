namespace PVCErp.Application.Dtos;

public sealed record DashboardDto(decimal RawMaterialStockKg, decimal ProductionApprovedKg, decimal ProductionRejectedKg, decimal ScrapGeneratedKg, decimal ScrapReusedKg, int PendingDispatchCount);
