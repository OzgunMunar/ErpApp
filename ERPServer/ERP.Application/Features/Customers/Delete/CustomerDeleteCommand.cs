using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Application.Features.Customers.Delete
{
    public sealed record CustomerDeleteCommand(
        Guid Id) : IRequest<Result<string>>;

    internal sealed class CustomerDeleteCommandHandler(
        
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork
        
        )
        : IRequestHandler<CustomerDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            
            Customer? customer = await customerRepository.GetAll()
                .Where(cus => cus.Id  == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null) 
            {
                return Result<string>.Failure(404, "Customer not found.");
            }

            customer.IsDeleted = false;

            customerRepository.Update(customer);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Customer deleted successfully.");

        }
    }
}
