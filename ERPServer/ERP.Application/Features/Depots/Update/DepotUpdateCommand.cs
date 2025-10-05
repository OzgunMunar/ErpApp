using ERP.Domain.Repositories;
using GenericRepository;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Depot.Update
{
    public sealed record DepotUpdateCommand(

        Guid Id,
        string DepotName,
        string City,
        string Town,
        string Street

        ) : IRequest<Result<string>>;

    internal sealed class DepotUpdateCommandHandler(
        
        IDepotRepository depotRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        
        )
        : IRequestHandler<DepotUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DepotUpdateCommand request, CancellationToken cancellationToken)
        {

            ERP.Domain.Entities.Depot? depot = await depotRepository
                    .Where(dep => 
                        dep.Id == request.Id
                        &&
                        dep.IsDeleted == false
                        )
                    .FirstOrDefaultAsync(cancellationToken);

            if(depot == null )
            {
                return Result<string>.Failure(404, "Depot not found");
            }

            mapper = new Mapper();
            mapper.Map(request, depot);

            depotRepository.Update(depot);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Depot successfully saved.");

        }

    }

}