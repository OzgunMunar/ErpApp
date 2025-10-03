using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Repositories
{
    public sealed class DepotRepository(ApplicationDbContext context) : Repository<Depot, ApplicationDbContext>(context), IDepotRepository
    {
    }
}
