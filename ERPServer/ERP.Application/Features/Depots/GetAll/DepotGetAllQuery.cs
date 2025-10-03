using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ERP.Application.Features.Depot.GetAll
{
    public sealed record DepotGetAllQuery(): IRequest<IQueryable<DepotGetAllQueryResponse>>;

    public sealed record DepotGetAllQueryResponse(

        Guid Id,
        string DepotName,
        string City,
        string Town,
        string Street

        );

    internal sealed class DepotGetAllQueryHandler(
        
        IDepotRepository depotRepository
        
        )
        : IRequestHandler<DepotGetAllQuery, IQueryable<DepotGetAllQueryResponse>>
    {
        public async Task<IQueryable<DepotGetAllQueryResponse>> Handle(DepotGetAllQuery request, CancellationToken cancellationToken)
        {
            
            var depots = await depotRepository
                .Where(depot => depot.IsDeleted == false)
                .Select(depot => new DepotGetAllQueryResponse(
                    
                    depot.Id,
                    depot.DepotName,
                    depot.City,
                    depot.Town,
                    depot.Street
                    
                    )
                )
                .ToListAsync(cancellationToken);

            return depots.AsQueryable();

        }
    }
}
