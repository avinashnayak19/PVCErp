using PVCErp.Application.Abstractions;
using PVCErp.Application.Dtos;
using PVCErp.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static PVCErp.Application.Dtos.SocketDto;

namespace PVCErp.Application.Services
{
    public interface ISocketingService
    {
        Task<SocketDto> CreateSocketingAsync(CreateSocketRequest request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SocketDto>> GetSocketingAsync(CancellationToken cancellationToken = default);

    }

    public sealed class SocketingService : ISocketingService
    {
        private readonly IRepository<SocketingEntry> _socketingentry;
        private readonly IUnitOfWork _unitOfWork;

        public SocketingService(IRepository<SocketingEntry> socketingentry, IUnitOfWork unitOfWork)
        {
            _socketingentry = socketingentry ?? throw new ArgumentNullException(nameof(socketingentry));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<SocketDto> CreateSocketingAsync(CreateSocketRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var socketingentry = new SocketingEntry
            {
                EntryDate = request.EntryDate,
                PipesReceived = request.PipesReceived,
                PipesSocketed = request.PipesSocketed,
                RejectedQty = request.RejectedQuantity,
                ScrapWeight = request.ScrapWeight,
                Shift = request.Shift
            };

            await _socketingentry.AddAsync(socketingentry, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SocketDto(
                socketingentry.EntryDate,
                socketingentry.PipesReceived,
                socketingentry.PipesSocketed,
                socketingentry.RejectedQty,
                socketingentry.ScrapWeight,
                socketingentry.Shift);
        }
        public async Task<IReadOnlyList<SocketDto>> GetSocketingAsync(CancellationToken cancellationToken = default)
        {
            var entries = await _socketingentry.ListAsync(cancellationToken);

            return entries
                .Select(e => new SocketDto(
                    e.EntryDate,
                    e.PipesReceived,
                    e.PipesSocketed,
                    e.RejectedQty,
                    e.ScrapWeight,
                    e.Shift))
                .OrderBy(e => e.EntryDate)
                .ToList();
        }

    }
}
