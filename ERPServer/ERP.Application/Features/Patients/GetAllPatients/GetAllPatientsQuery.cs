using TS.MediatR;

namespace ERPServer.Application.Features.Patients.GetAllPatients;

public sealed record GetAllPatientsQuery():
    IRequest<IQueryable<GetAllPatientsQueryResponse>>;
