using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

using TS.Result;

namespace ERP.Application.Features.Depot.Delete
{
    public sealed record DepotDeleteCommand(Guid Id):IRequest<Result<string>>;

    internal sealed class DepotDeleteCommandHandler(
        
        IDepotRepository depotRepository,
        IUnitOfWork unitOfWork
        
        )
        : IRequestHandler<DepotDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DepotDeleteCommand request, CancellationToken cancellationToken)
        {

            var depot = await depotRepository
                .GetAll()
                .Where(depot  => 
                    depot.Id == request.Id 
                    &&
                    depot.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);

            if(depot == null)
            {
                return Result<string>.Failure(404, "Depot not found");
            }

            depot.IsDeleted = true;

            depotRepository.Update(depot);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<string>.Succeed("Depot successfully deleted");

        }

    }

}