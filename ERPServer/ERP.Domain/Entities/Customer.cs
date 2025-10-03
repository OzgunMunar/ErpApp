using ERP.Domain.Abstractions;

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
