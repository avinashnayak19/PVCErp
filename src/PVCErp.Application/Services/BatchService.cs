using PVCErp.Application.Abstractions;
using PVCErp.Application.Dtos;
using PVCErp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCErp.Application.Services
{
    public interface IBatchService
    {
        Task<IReadOnlyList<RawMaterialDto>> GetStockAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<GrnGetDto>> GetGrnAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RawMaterialDto>> GetRawMaterialAsync(CancellationToken cancellationToken = default);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default);
        Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialRequest request, CancellationToken cancellationToken = default);
        Task<GrnDto> CreateGrnAsync(CreateGrnRequest request, CancellationToken cancellationToken = default);
    }
    public sealed class BatchService : IBatchService
    {
        private readonly IRepository<RawMaterial> _rawMaterials;
        private readonly IRepository<Supplier> _suppliers;
        private readonly IRepository<GoodsReceiptNote> _grns;
        private readonly IRepository<InventoryBatch> _inventoryBatches;
        private readonly IUnitOfWork _unitOfWork;

        public BatchService(
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
        public Task<GrnDto> CreateGrnAsync(CreateGrnRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RawMaterialDto> CreateRawMaterialAsync(CreateRawMaterialRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<GrnGetDto>> GetGrnAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<RawMaterialDto>> GetRawMaterialAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<RawMaterialDto>> GetStockAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
