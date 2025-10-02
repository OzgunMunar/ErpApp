using ERP.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Reflection;

namespace ERP.WebAPI.Controllers
{
    [Route("odata")]
    [ApiController]
    [EnableQuery]
    [Authorize]
    public class AppODataController : ODataController
    {
        private readonly IMediator _mediator;

        public AppODataController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();

            builder.EnableLowerCamelCase();

            //builder.EntitySet<GetAllDoctorsQueryResponse>("doctors");
            //builder.EntitySet<AppRole>("roles");

            return builder.GetEdmModel();
        }

        //[HttpGet("employees")]
        //public async Task<IQueryable<GetAllDoctorsQueryResponse>> GetAllDoctors(CancellationToken cancellationToken)
        //{
        //    var response = await sender.Send(new GetAllDoctorsQuery(), cancellationToken);
        //    return response;
        //}

    }
}
