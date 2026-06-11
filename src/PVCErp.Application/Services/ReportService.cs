using PVCErp.Application.Abstractions;
using PVCErp.Application.Dtos;
using PVCErp.Domain.Common;
using PVCErp.Domain.Entities;

namespace PVCErp.Application.Services;

public interface IReportService
{
    Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
}

public sealed class ReportService(
    IRepository<InventoryBatch> inventoryBatches,
    IRepository<ProductionOutput> productionOutputs,
    IRepository<ScrapRecord> scrapRecords,
    IRepository<DispatchChallan> dispatches) : IReportService
{
    public async Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var stock = await inventoryBatches.ListAsync(cancellationToken);
        var outputs = await productionOutputs.ListAsync(cancellationToken);
        var scrap = await scrapRecords.ListAsync(cancellationToken);
        var allDispatches = await dispatches.ListAsync(cancellationToken);

        return new DashboardDto(
            stock.Sum(item => item.AvailableQuantityKg),
            outputs.Sum(item => item.ApprovedQuantityKg),
            outputs.Sum(item => item.RejectedQuantityKg),
            scrap.Sum(item => item.GeneratedKg),
            scrap.Sum(item => item.ReusedKg),
            allDispatches.Count(item => item.Status is DispatchStatus.Pending or DispatchStatus.PartiallyDispatched));
    }
}
