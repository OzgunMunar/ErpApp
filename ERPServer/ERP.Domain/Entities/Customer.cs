using ERP.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entities
{
    public sealed class Customer : Entity
    {

        public string FullName { get; set; } = default!;
        public string TaxDepartment { get; set; } = default!;
        public string TaxNumber { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Town { get; set; } = default!;
        public string Street { get; set; } = default!;


    }
}
