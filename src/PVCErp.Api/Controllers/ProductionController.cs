using Microsoft.AspNetCore.Mvc;
using PVCErp.Application.Dtos;
using PVCErp.Application.Services;

namespace PVCErp.Api.Controllers;

[ApiController]
[Route("api/production")]
public sealed class ProductionController(IProductionService productionService) : ControllerBase
{
    [HttpPost("machines")]
    public async Task<ActionResult<MachineDto>> CreateMachine(CreateMachineRequest request, CancellationToken cancellationToken) =>
        Ok(await productionService.CreateMachineAsync(request, cancellationToken));

    [HttpPost("screw-barrels")]
    public async Task<ActionResult<ScrewBarrelDto>> CreateScrewBarrel(CreateScrewBarrelRequest request, CancellationToken cancellationToken) =>
        Ok(await productionService.CreateScrewBarrelAsync(request, cancellationToken));

    [HttpPost("formulas")]
    public async Task<ActionResult<BatchFormulaDto>> CreateFormula(CreateBatchFormulaRequest request, CancellationToken cancellationToken) =>
        Ok(await productionService.CreateFormulaAsync(request, cancellationToken));

    [HttpPost("batches")]
    public async Task<ActionResult<ProductionBatchDto>> CreateBatch(CreateProductionBatchRequest request, CancellationToken cancellationToken) =>
        Ok(await productionService.CreateBatchAsync(request, cancellationToken));

    [HttpPost("scrap")]
    public async Task<ActionResult<ScrapDto>> RecordScrap(ScrapRequest request, CancellationToken cancellationToken) =>
        Ok(await productionService.RecordScrapAsync(request, cancellationToken));

    [HttpGet("screw-barrel-efficiency")]
    public async Task<ActionResult<IReadOnlyList<EfficiencyDto>>> GetScrewBarrelEfficiency(CancellationToken cancellationToken) =>
        Ok(await productionService.GetScrewBarrelEfficiencyAsync(cancellationToken));
}
