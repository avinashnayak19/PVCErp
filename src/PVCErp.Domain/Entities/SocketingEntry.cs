using PVCErp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCErp.Domain.Entities
{
    public sealed class SocketingEntry : BaseEntity
    {
        public int Id { get; set; }
        public int PipesReceived { get; set; }
        public int PipesSocketed { get; set; }
        public int RejectedQty { get; set; }
        public string Shift { get; set; }
        public decimal ScrapWeight { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
