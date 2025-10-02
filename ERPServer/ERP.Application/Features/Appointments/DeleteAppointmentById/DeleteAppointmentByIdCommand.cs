using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using GenericRepository;
using TS.MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Appointments.DeletERPById;

public sealed record DeletERPByIdCommand(

    Guid Id

): IRequest<Result<string>>;

internal sealed class DeletERPByIdCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
)
: IRequestHandler<DeletERPByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletERPByIdCommand request, CancellationToken cancellationToken)
    {

        Appointment? appointment = await appointmentRepository
                            .FirstOrDefaultAsync(appo => appo.Id == request.Id, cancellationToken);

        if (appointment == null)
        {

            return Result<string>.Failure("Appointment could not found");

        }

        if (appointment.IsCompleted == true)
        {
            return Result<string>.Failure("Completed appointments could not be deleted.");
        }

        appointment.IsActive = false;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Appointment is deleted");

    }
    
}