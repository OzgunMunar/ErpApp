using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ERP.Application.Features.Products.Delete
{
    public sealed record ProductDeleteCommand(Guid Id) : IRequest<Result<string>>;

    internal sealed class ProductDeleteCommandHandler(

        IProductRepository productRepository,
        IUnitOfWork unitOfWork

        ) : IRequestHandler<ProductDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {

            Product? product = await productRepository
                .Where(prod => prod.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null) 
            {
                return Result<string>.Failure(404, "Product not found.");
            }

            product.IsDeleted = true;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Product successfully deleted.");

        }

    }

}