using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Products.Create
{
    public sealed record ProductCreateCommand(
        
        string ProductName,
        int ProductType
        
        ) : IRequest<Result<string>>;

    internal sealed class ProductCreateCommandHandler(

        IProductRepository productRepository,
        IUnitOfWork unitOfWork

        ) : IRequestHandler<ProductCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {

            Product? product = await productRepository
                .Where(pro => pro.ProductName == request.ProductName)
                .FirstOrDefaultAsync(cancellationToken);

            if (product != null) 
            {
                return Result<string>.Failure("Product with the same name exists.");
            }

            var config = new TypeAdapterConfig();
            config.NewConfig<ProductCreateCommand, Product>()
                .Map(dest => dest.ProductType, src => ProductEnum.FromValue(src.ProductType));

            Product newProduct = request.Adapt<Product>(config);

            productRepository.Add(newProduct);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Product is saved successfully");

        }
    }

}
