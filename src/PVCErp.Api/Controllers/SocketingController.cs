using Microsoft.AspNetCore.Mvc;
using PVCErp.Application.Dtos;
using PVCErp.Application.Services;
using PVCErp.Domain.Entities;
using PVCErp.Infrastructure.Persistence;

namespace PVCErp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketingController(ISocketingService socketingservice) : ControllerBase
    {
        [HttpPost("insertsocket")]
        public async Task<ActionResult<RawMaterialDto>> CreateSocket(CreateSocketRequest request, CancellationToken cancellationToken) =>
         Ok(await socketingservice.CreateSocketingAsync(request, cancellationToken));
        [HttpGet("GetSocketing")]
        public async Task<ActionResult<IReadOnlyList<RawMaterialDto>>> GetSocketing(CancellationToken cancellationToken) =>
       Ok(await socketingservice.GetSocketingAsync(cancellationToken));
       
    }
}
