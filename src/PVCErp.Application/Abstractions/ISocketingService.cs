using PVCErp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCErp.Application.Abstractions
{
    public interface ISocketingService
    {
        Task<List<SocketingEntry>> GetAll();
        Task<SocketingEntry> GetById(int id);
        Task<SocketingEntry> Create(SocketingEntry model);
        Task<SocketingEntry> Update(int id, SocketingEntry model);
        Task<bool> Delete(int id);
    }
}
