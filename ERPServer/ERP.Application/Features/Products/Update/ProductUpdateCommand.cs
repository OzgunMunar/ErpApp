using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MapsterMapper;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Products.Update
{
    public sealed record ProductUpdateCommand
    (

        Guid Id,
        string ProductName,
        int ProductType

    ) : IRequest<Result<string>>;

    internal sealed class ProductUpdateCommandHandler(

        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper

    ) : IRequestHandler<ProductUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {

            Product? product = await productRepository
                .FirstOrDefaultAsync(product =>
                product.Id == request.Id
                &&
                product.IsDeleted == false, cancellationToken);

            if (product == null) 
            {
                return Result<string>.Failure(404, "Product not found.");
            }

            var config = new TypeAdapterConfig();
            config.NewConfig<ProductUpdateCommand, Product>()
                .Map(dest => dest.ProductType, src => ProductEnum.FromValue(src.ProductType));

            mapper = new Mapper(config);
            mapper.Map(request, product);

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Product is updated successfully");


        }

    }

}