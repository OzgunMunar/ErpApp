namespace ERP.Domain.Dtos
{
    public sealed record InvoiceDetailDto
    (
        Guid ProductId,
        decimal Quantity,
        decimal Price,
        Guid DepotId
    );
}
