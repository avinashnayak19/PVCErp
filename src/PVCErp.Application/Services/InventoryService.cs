using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PVCErp.Application.Abstractions;
using PVCErp.Application.Dtos;
using PVCErp.Domain.Entities;

namespace PVCErp.Application.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<RawMaterialDto>> GetStockAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GrnGetDto>> GetGrnAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RawMaterialDto>> GetRawMaterialAsync(CancellationToken cancellationToken = default);
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default);
    Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialRequest request, CancellationToken cancellationToken = default);
    Task<GrnDto> CreateGrnAsync(CreateGrnRequest request, CancellationToken cancellationToken = default);
}

public sealed class InventoryService : IInventoryService
{
    private readonly IRepository<RawMaterial> _rawMaterials;
    private readonly IRepository<Supplier> _suppliers;
    private readonly IRepository<GoodsReceiptNote> _grns;
    private readonly IRepository<InventoryBatch> _inventoryBatches;
    private readonly IUnitOfWork _unitOfWork;

    public InventoryService(
        IRepository<RawMaterial> rawMaterials,
        IRepository<Supplier> suppliers,
        IRepository<GoodsReceiptNote> grns,
        IRepository<InventoryBatch> inventoryBatches,
        IUnitOfWork unitOfWork)
    {
        _rawMaterials = rawMaterials ?? throw new ArgumentNullException(nameof(rawMaterials));
        _suppliers = suppliers ?? throw new ArgumentNullException(nameof(suppliers));
        _grns = grns ?? throw new ArgumentNullException(nameof(grns));
        _inventoryBatches = inventoryBatches ?? throw new ArgumentNullException(nameof(inventoryBatches));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IReadOnlyList<RawMaterialDto>> GetStockAsync(CancellationToken cancellationToken = default)
    {
        var materials = await _rawMaterials.ListAsync(cancellationToken);
        var batches = await _inventoryBatches.ListAsync(cancellationToken);

        return materials
            .Select(material => new RawMaterialDto(
                material.Id,
                material.Name,
                material.Unit,
                material.ReorderLevel,
                batches.Where(batch => batch.RawMaterialId == material.Id).Sum(batch => batch.AvailableQuantityKg)))
            .OrderBy(item => item.Name)
            .ToList();
    }

    public async Task<IReadOnlyList<RawMaterialDto>> GetRawMaterialAsync(CancellationToken cancellationToken = default)
    {
        var materials = await _rawMaterials.ListAsync(cancellationToken);
        var batches = await _inventoryBatches.ListAsync(cancellationToken);

        return materials
            .Select(material => new RawMaterialDto(
                material.Id,
                material.Name,
                material.Unit,
                material.ReorderLevel,
                batches.Where(batch => batch.RawMaterialId == material.Id).Sum(batch => batch.AvailableQuantityKg)))
            .OrderBy(item => item.Name)
            .ToList();
    }

    public async Task<IReadOnlyList<GrnGetDto>> GetGrnAsync(CancellationToken cancellationToken = default)
    {
        var grnList = await _grns.ListAsync(cancellationToken);
        var batchList = await _inventoryBatches.ListAsync(cancellationToken);
        var materialList = await _rawMaterials.ListAsync(cancellationToken);
        var supplierList = await _suppliers.ListAsync(cancellationToken);

        var result =
            (
                from grn in grnList
                join supplier in supplierList on grn.SupplierId equals supplier.Id
                join batch in batchList on supplier.Id equals batch.SupplierId
                join material in materialList on batch.RawMaterialId equals material.Id
                select new GrnGetDto(
                    grn.Id,
                    grn.GrnNumber,
                    supplier.Id,
                    supplier.Name,
                    grn.ReceiptDate,
                    material.Name,
                    $"{batch.ReceivedQuantityKg} Kg",
                    batch.QualityStatus.ToString()
                )
            )
            .OrderByDescending(x => x.Date)
            .ToList();

        return result;
    }

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        var supplier = new Supplier
        {
            Name = request.Name,
            Country = request.Country,
            GstNumber = request.GstNumber,
            Phone = request.Phone
        };

        await _suppliers.AddAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SupplierDto(supplier.Id, supplier.Name, supplier.Country, supplier.GstNumber, supplier.Phone);
    }

    public async Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialRequest request, CancellationToken cancellationToken = default)
    {
        var rawMaterial = new RawMaterial
        {
            Name = request.Name,
            Unit = request.Unit,
            ReorderLevel = request.ReorderLevel
        };

        await _rawMaterials.AddAsync(rawMaterial, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RawMaterialDto(rawMaterial.Id, rawMaterial.Name, rawMaterial.Unit, rawMaterial.ReorderLevel, 0);
    }

    public async Task<GrnDto> CreateGrnAsync(CreateGrnRequest request, CancellationToken cancellationToken = default)
    {
        var grn = new GoodsReceiptNote
        {
            GrnNumber = $"GRN-{DateTime.UtcNow:yyyyMMddHHmmss}",
            SupplierId = request.SupplierId,
            ReceiptDate = request.ReceiptDate,
            QualityStatus = request.QualityStatus,
            Items = request.Items.Select(item => new GoodsReceiptItem
            {
                RawMaterialId = item.RawMaterialId,
                BatchNumber = item.BatchNumber,
                QuantityKg = item.QuantityKg
            }).ToList()
        };

        await _grns.AddAsync(grn, cancellationToken);

        foreach (var item in request.Items)
        {
            await _inventoryBatches.AddAsync(new InventoryBatch
            {
                RawMaterialId = item.RawMaterialId,
                SupplierId = request.SupplierId,
                BatchNumber = item.BatchNumber,
                ReceivedQuantityKg = item.QuantityKg,
                AvailableQuantityKg = item.QuantityKg,
                QualityStatus = request.QualityStatus
            }, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new GrnDto(grn.Id, grn.GrnNumber, grn.SupplierId, grn.ReceiptDate, grn.QualityStatus);
    }
}
