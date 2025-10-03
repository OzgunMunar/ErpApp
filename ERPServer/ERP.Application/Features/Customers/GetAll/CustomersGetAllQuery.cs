using ERP.Domain.Abstractions;
using ERP.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Features.Customers.GetAll
{
    public sealed record CustomersGetAllQuery()
        :IRequest<IQueryable<CustomersGetAllQueryResponse>>;

    public sealed class CustomersGetAllQueryResponse() : EntityDto
    {

        public string FullName { get; set; } = default!;
        public string TaxDepartment { get; set; } = default!;
        public string TaxNumber { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Town { get; set; } = default!;
        public string Street { get; set; } = default!;

    }


    internal sealed class CustomersGetAllQueryHandler(

        ICustomerRepository customerRepository

        ) : IRequestHandler<CustomersGetAllQuery, IQueryable<CustomersGetAllQueryResponse>>
    {
        public async Task<IQueryable<CustomersGetAllQueryResponse>> Handle(CustomersGetAllQuery request, CancellationToken cancellationToken)
        {

            var customers = await (

                    from customer in customerRepository.GetAll()

                    where customer.IsDeleted == false

                    select new CustomersGetAllQueryResponse
                    {
                        Id = customer.Id,
                        FullName = customer.FullName,
                        TaxDepartment = customer.TaxDepartment,
                        TaxNumber = customer.TaxNumber,
                        City = customer.City,
                        Town = customer.Town,
                        Street = customer.Street,
                    }


               ).ToListAsync(cancellationToken);

            return customers.AsQueryable();

        }

    }

}