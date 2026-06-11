using Microsoft.AspNetCore.Mvc;
using PVCErp.Application.Dtos;
using PVCErp.Application.Services;

namespace PVCErp.Api.Controllers;

[ApiController]
[Route("api/dispatch")]
public sealed class DispatchController(IDispatchService dispatchService) : ControllerBase
{
    [HttpPost("customers")]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken) =>
        Ok(await dispatchService.CreateCustomerAsync(request, cancellationToken));

    [HttpPost("challans")]
    public async Task<ActionResult<DispatchDto>> CreateDispatch(CreateDispatchRequest request, CancellationToken cancellationToken) =>
        Ok(await dispatchService.CreateDispatchAsync(request, cancellationToken));

    [HttpPost("invoices")]
    public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceRequest request, CancellationToken cancellationToken) =>
        Ok(await dispatchService.CreateInvoiceAsync(request, cancellationToken));
}
