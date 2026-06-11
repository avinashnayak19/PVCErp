using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVCErp.Application.Dtos
{
    public sealed record SocketDto(DateTime EntryDate, int PipesReceived, int PipesSocketed, int RejectedQuantity, decimal ScrapWeight, string Shift)
    {
        private object id;
        private object name;
        private object unit;
        private object reorderLevel;
        private object value;

        public SocketDto(object id, object name, object unit, object reorderLevel, object value)
            : this(default, default, default, default, default, default)
        {
            this.id = id;
            this.name = name;
            this.unit = unit;
            this.reorderLevel = reorderLevel;
            this.value = value;
        }
    }

    public sealed record CreateSocketRequest(DateTime EntryDate, int PipesReceived, int PipesSocketed, int RejectedQuantity, decimal ScrapWeight, string Shift);
}
