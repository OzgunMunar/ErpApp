using Ardalis.SmartEnum;

namespace ERP.Domain.Enums
{
    public sealed class ProductEnum(string productName, int value) : SmartEnum<ProductEnum>(productName, value)
    {

        public static readonly ProductEnum FinishedProduct = new("Finished Product", 1);
        public static readonly ProductEnum SemiFinishedProduct = new("Semi-Finished Product", 2);

    }

}
