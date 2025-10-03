using ERP.Domain.Abstractions;

namespace ERP.Domain.Entities
{
    public sealed class Depot : Entity
    {
        public string DepotName { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Town { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
