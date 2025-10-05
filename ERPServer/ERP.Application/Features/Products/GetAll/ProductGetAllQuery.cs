using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Features.Products.GetAll
{
    public sealed record ProductGetAllQuery()
        : IRequest<IQueryable<ProductGetAllQueryResponse>>;

    public sealed record ProductGetAllQueryResponse
        (
            Guid Id,
            string ProductName,
            int ProductType
        );

    internal sealed class ProductGetAllQueryHandler(
        
        IProductRepository productRepository
        
        )
        : IRequestHandler<ProductGetAllQuery, IQueryable<ProductGetAllQueryResponse>>
    {
        public async Task<IQueryable<ProductGetAllQueryResponse>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
        {
            
            var products = await productRepository
                .Where(product => product.IsDeleted == false)
                .Select(product => new ProductGetAllQueryResponse
                 (
                    product.Id,
                    product.ProductName,
                    product.ProductType
                 ))
                .ToListAsync(cancellationToken);

            return products.AsQueryable();

        }

    }

}