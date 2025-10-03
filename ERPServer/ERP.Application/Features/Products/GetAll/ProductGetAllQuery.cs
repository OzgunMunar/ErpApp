using ERP.Domain.Abstractions;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Products.GetAll
{
    public sealed record ProductGetAllQuery()
        : IRequest<IQueryable<ProductGetAllQueryResponse>>;

    public sealed class ProductGetAllQueryResponse : EntityDto
    {
        public string ProductName { get; set; } = default!;
        public int ProductType { get; set; }

    }

    internal sealed class ProductGetAllQueryHandler(
        
        IProductRepository productRepository
        
        )
        : IRequestHandler<ProductGetAllQuery, IQueryable<ProductGetAllQueryResponse>>
    {
        public async Task<IQueryable<ProductGetAllQueryResponse>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
        {
            
            var products = await productRepository
                .GetAll()
                .Where(product => product.IsDeleted == false)
                .Select(product => new ProductGetAllQueryResponse
                {
                    ProductName = product.ProductName,
                    ProductType = product.ProductType
                })
                .ToListAsync(cancellationToken);

            return products.AsQueryable();

        }

    }

}