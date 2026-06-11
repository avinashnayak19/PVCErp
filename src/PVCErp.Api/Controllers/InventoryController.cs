using Microsoft.AspNetCore.Mvc;
using PVCErp.Application.Dtos;
using PVCErp.Application.Services;

namespace PVCErp.Api.Controllers;

[ApiController]
[Route("api/inventory")] 
public sealed class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpGet("stock")]
    public async Task<ActionResult<IReadOnlyList<RawMaterialDto>>> GetStock(CancellationToken cancellationToken) =>
        Ok(await inventoryService.GetStockAsync(cancellationToken));
    [HttpGet("GetGrn")]
    public async Task<ActionResult<IReadOnlyList<RawMaterialDto>>> GetGrn(CancellationToken cancellationToken) =>
        Ok(await inventoryService.GetGrnAsync(cancellationToken));

    [HttpPost("raw-materials")]
    public async Task<ActionResult<RawMaterialDto>> CreateRawMaterial(CreateRawMaterialRequest request, CancellationToken cancellationToken) =>
        Ok(await inventoryService.CreateRawMaterialAsync(request, cancellationToken));

    [HttpPost("suppliers")]
    public async Task<ActionResult<SupplierDto>> CreateSupplier(CreateSupplierRequest request, CancellationToken cancellationToken) =>
        Ok(await inventoryService.CreateSupplierAsync(request, cancellationToken));

    [HttpPost("grns")]
    public async Task<ActionResult<GrnDto>> CreateGrn(CreateGrnRequest request, CancellationToken cancellationToken) =>
        Ok(await inventoryService.CreateGrnAsync(request, cancellationToken));

    [HttpGet("get-materials")]
    public async Task<ActionResult<RawMaterialDto>> GetRawMaterial(CancellationToken cancellationToken) =>
        Ok(await inventoryService.GetRawMaterialAsync(cancellationToken));

}
