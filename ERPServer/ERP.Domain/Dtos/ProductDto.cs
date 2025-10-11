using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Dtos
{
    public sealed record ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
        
        
}
