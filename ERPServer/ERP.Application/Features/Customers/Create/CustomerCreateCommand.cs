using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace ERP.Application.Features.Customers.Create
{
    public sealed record CustomerCreateCommand(

        string FullName,
        string TaxDepartment,
        string TaxNumber,
        string City,
        string Town,
        string Street): IRequest<Result<string>>;

    internal sealed class CustomerCreateCommandHandler(

        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork

        ) : IRequestHandler<CustomerCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {

            Customer? customer = await customerRepository
                                    .GetAll()
                                    .Where(customer => customer.TaxNumber == request.TaxNumber)
                                    .FirstOrDefaultAsync(cancellationToken);

            if ( customer != null)
            {
                return Result<string>.Failure(409, "There is a customer with the same Tax Number.");
            }

            Customer newCostomer = request.Adapt<Customer>();
            
            await customerRepository.AddAsync(newCostomer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<string>.Succeed("Customer saved successfully.");

        }
    }

}
