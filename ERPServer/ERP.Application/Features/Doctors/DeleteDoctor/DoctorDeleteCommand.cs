using Mapster;
using Microsoft.AspNetCore.Http;
using TS.MediatR;
using TS.Result;

namespace ERP.Application.Doctors.CreateDoctor;    

public sealed record DoctorDeleteCommand(
    Guid Id
) : IRequest<Result<string>>;
