using Microsoft.AspNetCore.Mvc;
using PVCErp.Application.Dtos;
using PVCErp.Application.Services;

namespace PVCErp.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> GetDashboard(CancellationToken cancellationToken) =>
        Ok(await reportService.GetDashboardAsync(cancellationToken));
}
