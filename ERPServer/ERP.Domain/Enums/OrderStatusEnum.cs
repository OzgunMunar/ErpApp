using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Enums
{
    public sealed class OrderStatusEnum(string name, int value) : SmartEnum<OrderStatusEnum>(name, value)
    {
        public static readonly OrderStatusEnum Pending = new("Pending", 1);
        public static readonly OrderStatusEnum RequirementPlanPending = new("Requirement Plan Pending", 2);
        public static readonly OrderStatusEnum Completed = new("Completed", 3);
    }
}
