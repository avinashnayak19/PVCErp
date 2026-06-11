using PVCErp.Application.Abstractions;
using PVCErp.Application.Dtos;
using PVCErp.Domain.Common;
using PVCErp.Domain.Entities;

namespace PVCErp.Application.Services;

public interface IDispatchService
{
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<DispatchDto> CreateDispatchAsync(CreateDispatchRequest request, CancellationToken cancellationToken = default);
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default);
}

public sealed class DispatchService(
    IRepository<Customer> customers,
    IRepository<DispatchChallan> dispatches,
    IRepository<Invoice> invoices,
    IUnitOfWork unitOfWork) : IDispatchService
{
    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = new Customer { Name = request.Name, GstNumber = request.GstNumber, Phone = request.Phone, Address = request.Address };
        await customers.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new CustomerDto(customer.Id, customer.Name, customer.GstNumber, customer.Phone, customer.Address);
    }

    public async Task<DispatchDto> CreateDispatchAsync(CreateDispatchRequest request, CancellationToken cancellationToken = default)
    {
        var dispatch = new DispatchChallan
        {
            ChallanNumber = $"DC-{DateTime.UtcNow:yyyyMMddHHmmss}",
            CustomerId = request.CustomerId,
            DispatchDate = request.DispatchDate,
            VehicleNumber = request.VehicleNumber,
            Status = string.IsNullOrWhiteSpace(request.VehicleNumber) ? DispatchStatus.Pending : DispatchStatus.Dispatched,
            Items = request.Items.Select(item => new DispatchItem
            {
                PipeDimension = item.PipeDimension,
                QuantityKg = item.QuantityKg,
                QuantityPieces = item.QuantityPieces
            }).ToList()
        };

        await dispatches.AddAsync(dispatch, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDispatchDto(dispatch);
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}",
            DispatchChallanId = request.DispatchChallanId,
            InvoiceDate = request.InvoiceDate,
            TaxableAmount = request.TaxableAmount,
            GstAmount = request.GstAmount,
            TotalAmount = request.TaxableAmount + request.GstAmount,
            PaymentStatus = PaymentStatus.Unpaid
        };

        await invoices.AddAsync(invoice, cancellationToken);
        var dispatch = await dispatches.GetByIdAsync(request.DispatchChallanId, cancellationToken);
        if (dispatch is not null)
        {
            dispatch.Status = DispatchStatus.Invoiced;
            dispatches.Update(dispatch);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new InvoiceDto(invoice.Id, invoice.InvoiceNumber, invoice.DispatchChallanId, invoice.TotalAmount, invoice.PaymentStatus);
    }

    private static DispatchDto ToDispatchDto(DispatchChallan dispatch) =>
        new(dispatch.Id, dispatch.ChallanNumber, dispatch.CustomerId, dispatch.DispatchDate, dispatch.Status, dispatch.VehicleNumber, dispatch.Items.Sum(item => item.QuantityKg));
}
