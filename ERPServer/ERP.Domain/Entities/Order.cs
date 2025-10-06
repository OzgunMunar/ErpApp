using ERP.Domain.Abstractions;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public sealed class Order : Entity
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string OrderNumber { get; set; } = default!;
        public DateOnly OrderedDate { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
        public List<OrderDetail>? Details { get; set; }
    }
}