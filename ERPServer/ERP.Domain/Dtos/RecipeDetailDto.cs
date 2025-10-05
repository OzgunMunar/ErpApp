namespace ERP.Domain.Dtos
{
    public sealed record RecipeDetailDto(
        Guid ProductId,
        decimal Quantity);
}
