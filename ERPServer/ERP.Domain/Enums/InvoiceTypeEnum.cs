using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Enums
{
    public sealed class InvoiceTypeEnum(string name, int value) : SmartEnum<InvoiceTypeEnum>(name, value)
    {
        public static readonly InvoiceTypeEnum Purchase = new("Purchase", 1);
        public static readonly InvoiceTypeEnum Sales = new("Sales", 2);
    }
}
