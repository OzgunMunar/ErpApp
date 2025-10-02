using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Users
{
    public sealed class AppUserRole:IdentityUserRole<Guid>
    {
    }
}
