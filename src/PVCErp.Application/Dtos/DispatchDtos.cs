using PVCErp.Domain.Common;

namespace PVCErp.Application.Dtos;

public sealed record CreateCustomerRequest(string Name, string? GstNumber, string? Phone, string? Address);
public sealed record CustomerDto(Guid Id, string Name, string? GstNumber, string? Phone, string? Address);
public sealed record DispatchItemRequest(string PipeDimension, decimal QuantityKg, int QuantityPieces);
public sealed record CreateDispatchRequest(Guid CustomerId, DateOnly DispatchDate, string? VehicleNumber, List<DispatchItemRequest> Items);
public sealed record DispatchDto(Guid Id, string ChallanNumber, Guid CustomerId, DateOnly DispatchDate, DispatchStatus Status, string? VehicleNumber, decimal QuantityKg);
public sealed record CreateInvoiceRequest(Guid DispatchChallanId, DateOnly InvoiceDate, decimal TaxableAmount, decimal GstAmount);
public sealed record InvoiceDto(Guid Id, string InvoiceNumber, Guid DispatchChallanId, decimal TotalAmount, PaymentStatus PaymentStatus);
