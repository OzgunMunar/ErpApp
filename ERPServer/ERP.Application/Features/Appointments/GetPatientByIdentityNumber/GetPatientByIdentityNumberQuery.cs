using ERP.Domain.Entities;
using TS.MediatR;
using TS.Result;

namespace ERP.Application.Features.Appointments.GetPatientByIdentityNumber;

public sealed record GetPatientByIdentityNumberQuery(
    string IdentityNumber
) : IRequest<Result<Patient>>;
