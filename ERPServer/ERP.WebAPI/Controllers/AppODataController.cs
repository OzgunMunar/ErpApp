using ERP.Application.Features.Customers.GetAll;
using ERP.Application.Features.Depot.GetAll;
using ERP.Application.Features.Orders.GetAll;
using ERP.Application.Features.Products.GetAll;
using ERP.Application.Features.Recipies.GetAll;
using ERP.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ERP.WebAPI.Controllers
{
    [Route("odata")]
    [ApiController]
    [EnableQuery]
    //[Authorize]
    [AllowAnonymous]
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

            builder.EntitySet<CustomersGetAllQueryResponse>("customers");
            builder.EntitySet<DepotGetAllQueryResponse>("depots");
            builder.EntitySet<ProductGetAllQueryResponse>("products");
            builder.EntitySet<Recipe>("recipies");
            builder.EntitySet<OrdersGetAllQueryResponse>("orders");
            //builder.EntitySet<AppRole>("roles");

            return builder.GetEdmModel();
        }

        [HttpGet("customers")]
        public async Task<IQueryable<CustomersGetAllQueryResponse>> GetAllCustomers(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CustomersGetAllQuery(), cancellationToken);
            return response;
        }

        [HttpGet("depots")]
        public async Task<IQueryable<DepotGetAllQueryResponse>> GetAllDepots(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DepotGetAllQuery(), cancellationToken);
            return response;
        }

        [HttpGet("products")]
        public async Task<IQueryable<ProductGetAllQueryResponse>> GetAllProducts(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ProductGetAllQuery(), cancellationToken);
            return response;
        }

        [HttpGet("recipies")]
        public async Task<IQueryable<Recipe>> GetAllRecipies(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetAllRecipeQuery(), cancellationToken);
            return response;
        }

        [HttpGet("orders")]
        public async Task<IQueryable<OrdersGetAllQueryResponse>> GetAllOrders(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new OrdersGetAllQuery(), cancellationToken);
            return response;
        }
        
    }
}
