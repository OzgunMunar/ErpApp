using ERP.Domain.Abstractions;
using ERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entities
{
    public sealed class Product : Entity
    {
        public string ProductName { get; set; } = default!;
        public ProductEnum ProductType { get; set; } = ProductEnum.FinishedProduct;
    }
}
