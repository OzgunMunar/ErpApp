using TS.MediatR;
using TS.Result;

namespace ERP.Application.Features.Patients.PatientDelete;

public sealed record PatientDeleteCommand(

    Guid Id

): IRequest<Result<string>>;
