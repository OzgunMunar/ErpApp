using ERP.Domain.Dtos;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Orders.RequirementsPlanningByOrderId
{
    public sealed record RequirementsPlanningByOrderIdCommand(
        
        Guid OrderId

        )
        : IRequest<Result<RequirementsPlanningByOrderIdResponse>>;

    public sealed record RequirementsPlanningByOrderIdResponse(
        
        DateOnly Date,
        string Title,
        List<ProductDto> Products
        
        );

    internal sealed class RequirementsPlanningByOrderIdHandler(
        
        IOrderRepository orderRepository,
        IStockMovementRepository stockMovementRepository,
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork
        
        )
        : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdResponse>>
    {
        public async Task<Result<RequirementsPlanningByOrderIdResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
        {

            Order? order = await orderRepository
                .Where(ord =>
                    ord.Id == request.OrderId
                    &&
                    ord.IsDeleted == false)
                .Include(ord => ord.Details!)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(cancellationToken);

            if ( order == null )
            {
                return Result<RequirementsPlanningByOrderIdResponse>.Failure(404, "Order not found");
            }

            List<ProductDto> productListNeedsToBeCreated = new();
            List<ProductDto> requirementPlanningProducts = new();

            if(order.Details is not null)
            {
                foreach (var item in order.Details)
                {
                    
                    var product = item.Product;
                    List<StockMovement> movements = await stockMovementRepository
                        .Where(stock => stock.ProductId == product!.Id)
                        .ToListAsync(cancellationToken);

                    decimal stock = movements.Sum(s => s.NumberOfEntry) - movements.Sum(s => s.NumberOfOutputs);

                    if(stock < item.Quantity)
                    {

                        ProductDto productToCreate = new()
                        {
                            Id = item.ProductId,
                            ProductName = product!.ProductName,
                            Quantity = item.Quantity - stock
                        };

                        productListNeedsToBeCreated.Add(productToCreate);

                    }

                }

                foreach (var item in productListNeedsToBeCreated)
                {
                    
                    Recipe? recipe = await recipeRepository
                        .Where(p => p.ProductId == item.Id)
                        .Include(p => p.Details!)
                        .ThenInclude(p => p.Product)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (recipe is not null && recipe.Details is not null)
                    {

                        foreach (var detail in recipe.Details)
                        {
                            
                            List<StockMovement> stockMovements = await stockMovementRepository
                                .Where(p => p.ProductId == detail.ProductId)
                                .ToListAsync(cancellationToken);

                            decimal stock = stockMovements.Sum(st => st.NumberOfEntry) -
                                stockMovements.Sum(st => st.NumberOfOutputs);

                            if (stock < detail.Quantity)
                            {
                                
                                ProductDto productThatsInNeed = new()
                                {
                                    Id = detail.ProductId,
                                    ProductName = detail.Product!.ProductName,
                                    Quantity = detail.Quantity - stock
                                };

                                requirementPlanningProducts.Add(productThatsInNeed);

                            }

                        }

                    }

                }

            }

            requirementPlanningProducts = requirementPlanningProducts
                .GroupBy(p => p.Id)
                .Select(g => new ProductDto
                {
                    Id = g.Key,
                    ProductName = g.First().ProductName,
                    Quantity = g.Sum(item => item.Quantity)
                })
                .ToList();

            order.Status = OrderStatusEnum.RequirementPlanWorked;
            orderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RequirementsPlanningByOrderIdResponse(
                DateOnly.FromDateTime(DateTime.Now),
                "Requirement Plan Of " + order.OrderNumber,
                requirementPlanningProducts);

        }
    }
}
