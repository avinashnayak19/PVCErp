using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PVCErp.Application.Dtos;
using PVCErp.Application.Services;

namespace PVCErp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BatchController(IBatchService batchService) : ControllerBase
    {
        [HttpPost("customers")]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CancellationToken cancellationToken) =>
        Ok(await batchService.GetStockAsync(cancellationToken));

    }
}
