using ERP.Application.Features.Customers.Update;
using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using MapsterMapper;
using MediatR;
using TS.Result;

namespace ERP.Application.Features.Customers.Update
{
    public sealed record CustomerUpdateCommand(
        
        Guid Id,
        string FullName,
        string TaxDepartment,
        string TaxNumber,
        string City,
        string Town,
        string Street

        ) : IRequest<Result<string>>;
}

internal sealed class CustomerUpdateCommandHandler(
    
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    
    )
    : IRequestHandler<CustomerUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {

        Customer? customer = await customerRepository
            .FirstOrDefaultAsync(cus => 
            cus.Id == request.Id
            &&
            cus.IsDeleted == false
            , cancellationToken);

        if (customer == null) 
        {
            return Result<string>.Failure(404, "Customer not found");
        }

        mapper = new Mapper();
        mapper.Map(request, customer);

        customerRepository.Update(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Customer successfully updated.");

    }

}