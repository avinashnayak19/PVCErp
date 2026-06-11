using PVCErp.Application.Abstractions;
using PVCErp.Application.Dtos;
using PVCErp.Domain.Common;
using PVCErp.Domain.Entities;

namespace PVCErp.Application.Services;

public interface IProductionService
{
    Task<MachineDto> CreateMachineAsync(CreateMachineRequest request, CancellationToken cancellationToken = default);
    Task<ScrewBarrelDto> CreateScrewBarrelAsync(CreateScrewBarrelRequest request, CancellationToken cancellationToken = default);
    Task<BatchFormulaDto> CreateFormulaAsync(CreateBatchFormulaRequest request, CancellationToken cancellationToken = default);
    Task<ProductionBatchDto> CreateBatchAsync(CreateProductionBatchRequest request, CancellationToken cancellationToken = default);
    Task<ScrapDto> RecordScrapAsync(ScrapRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EfficiencyDto>> GetScrewBarrelEfficiencyAsync(CancellationToken cancellationToken = default);
}

public sealed class ProductionService(
    IRepository<BatchFormula> formulas,
    IRepository<ProductionBatch> productionBatches,
    IRepository<InventoryBatch> inventoryBatches,
    IRepository<ScrapRecord> scrapRecords,
    IRepository<ProductionOutput> outputs,
    IRepository<Machine> machines,
    IRepository<ScrewBarrel> screwBarrels,
    IUnitOfWork unitOfWork) : IProductionService
{
    public async Task<MachineDto> CreateMachineAsync(CreateMachineRequest request, CancellationToken cancellationToken = default)
    {
        var machine = new Machine { MachineNumber = request.MachineNumber, Description = request.Description };
        await machines.AddAsync(machine, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new MachineDto(machine.Id, machine.MachineNumber, machine.Description);
    }

    public async Task<ScrewBarrelDto> CreateScrewBarrelAsync(CreateScrewBarrelRequest request, CancellationToken cancellationToken = default)
    {
        var screwBarrel = new ScrewBarrel { BarrelNumber = request.BarrelNumber, Type = request.Type, TargetKgPerHour = request.TargetKgPerHour };
        await screwBarrels.AddAsync(screwBarrel, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new ScrewBarrelDto(screwBarrel.Id, screwBarrel.BarrelNumber, screwBarrel.Type, screwBarrel.TargetKgPerHour);
    }

    public async Task<BatchFormulaDto> CreateFormulaAsync(CreateBatchFormulaRequest request, CancellationToken cancellationToken = default)
    {
        var formula = new BatchFormula
        {
            FormulaCode = request.FormulaCode,
            Description = request.Description,
            Items = request.Items.Select(item => new BatchFormulaItem
            {
                RawMaterialId = item.RawMaterialId,
                StandardQuantityKg = item.StandardQuantityKg
            }).ToList()
        };

        await formulas.AddAsync(formula, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new BatchFormulaDto(formula.Id, formula.FormulaCode, formula.Description, formula.Items.Count);
    }

    public async Task<ProductionBatchDto> CreateBatchAsync(CreateProductionBatchRequest request, CancellationToken cancellationToken = default)
    {
        var batch = new ProductionBatch
        {
            BatchNumber = $"PB-{DateTime.UtcNow:yyyyMMddHHmmss}",
            BatchFormulaId = request.BatchFormulaId,
            ProductionDate = request.ProductionDate,
            Shift = request.Shift,
            OperatorName = request.OperatorName,
            SupervisorName = request.SupervisorName,
            Consumptions = request.Consumptions.Select(item => new ProductionConsumption
            {
                RawMaterialId = item.RawMaterialId,
                StandardQuantityKg = item.StandardQuantityKg,
                ActualQuantityKg = item.ActualQuantityKg
            }).ToList(),
            Outputs = request.Outputs.Select(item => new ProductionOutput
            {
                MachineId = item.MachineId,
                ScrewBarrelId = item.ScrewBarrelId,
                Stage = item.Stage,
                PipeDimension = item.PipeDimension,
                ApprovedQuantityKg = item.ApprovedQuantityKg,
                RejectedQuantityKg = item.RejectedQuantityKg,
                HoursRun = item.HoursRun
            }).ToList()
        };

        await productionBatches.AddAsync(batch, cancellationToken);
        await DeductInventoryAsync(request.Consumptions, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProductionBatchDto(
            batch.Id,
            batch.BatchNumber,
            batch.ProductionDate,
            batch.Shift,
            batch.OperatorName,
            batch.Outputs.Sum(output => output.ApprovedQuantityKg),
            batch.Outputs.Sum(output => output.RejectedQuantityKg));
    }

    public async Task<ScrapDto> RecordScrapAsync(ScrapRequest request, CancellationToken cancellationToken = default)
    {
        var scrap = new ScrapRecord
        {
            Stage = request.Stage,
            ScrapType = request.ScrapType,
            GeneratedKg = request.GeneratedKg,
            ReusedKg = request.ReusedKg,
            SourceReference = request.SourceReference,
            RecordDate = request.RecordDate
        };

        await scrapRecords.AddAsync(scrap, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToScrapDto(scrap);
    }

    public async Task<IReadOnlyList<EfficiencyDto>> GetScrewBarrelEfficiencyAsync(CancellationToken cancellationToken = default)
    {
        var allOutputs = await outputs.ListAsync(cancellationToken);
        var barrels = await screwBarrels.ListAsync(cancellationToken);

        return barrels.Select(barrel =>
        {
            var barrelOutputs = allOutputs.Where(output => output.ScrewBarrelId == barrel.Id).ToList();
            var produced = barrelOutputs.Sum(output => output.ApprovedQuantityKg);
            var hours = barrelOutputs.Sum(output => output.HoursRun);
            return new EfficiencyDto(barrel.BarrelNumber, produced, hours, hours == 0 ? 0 : produced / hours, barrel.TargetKgPerHour);
        }).ToList();
    }

    private async Task DeductInventoryAsync(IEnumerable<ConsumptionRequest> consumptions, CancellationToken cancellationToken)
    {
        var batches = (await inventoryBatches.ListAsync(cancellationToken))
            .Where(batch => batch.QualityStatus == QualityStatus.Approved)
            .OrderBy(batch => batch.CreatedAtUtc)
            .ToList();

        foreach (var consumption in consumptions)
        {
            var remaining = consumption.ActualQuantityKg;
            foreach (var batch in batches.Where(batch => batch.RawMaterialId == consumption.RawMaterialId && batch.AvailableQuantityKg > 0))
            {
                var used = Math.Min(batch.AvailableQuantityKg, remaining);
                batch.AvailableQuantityKg -= used;
                remaining -= used;
                inventoryBatches.Update(batch);
                if (remaining <= 0) break;
            }

            if (remaining > 0)
            {
                throw new InvalidOperationException("Insufficient raw material stock for production batch.");
            }
        }
    }

    private static ScrapDto ToScrapDto(ScrapRecord scrap) =>
        new(scrap.Id, scrap.Stage, scrap.ScrapType, scrap.GeneratedKg, scrap.ReusedKg, scrap.GeneratedKg == 0 ? 0 : scrap.ReusedKg / scrap.GeneratedKg * 100);
}
