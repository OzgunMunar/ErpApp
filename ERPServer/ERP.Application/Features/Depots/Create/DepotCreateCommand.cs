using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Depot.Create
{
    public sealed record DepotCreateCommand(

        string DepotName,
        string City,
        string Town,
        string Street

    ) : IRequest<Result<string>>;

    internal sealed class DepotCreateCommandHandler(

        IDepotRepository depotRepository,
        IUnitOfWork unitOfWork

        )
        : IRequestHandler<DepotCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DepotCreateCommand request, CancellationToken cancellationToken)
        {

            var depot = await depotRepository.GetAll()
                .Where(dep =>
                    dep.City == request.City
                    &&
                    dep.Town == request.Town
                    &&
                    dep.Street == request.Street
                    &&
                    dep.IsDeleted == false
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (depot != null) 
            {
                return Result<string>.Failure(409, "Depot with the same address already exist.");
            }

            ERP.Domain.Entities.Depot newDepot = request.Adapt<ERP.Domain.Entities.Depot>();

            await depotRepository.AddAsync(newDepot);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Depot successfully created.");


        }
    }

}
