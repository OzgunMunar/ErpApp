using ERP.Domain.Entities;
using TS.MediatR;
using TS.Result;

namespace ERP.Application.Features.Appointments.GetAllAppointmentsByDoctorId;

public sealed record GetAllAppointmentsByDoctorIdQuery(
    Guid DoctorId
):IRequest<Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>>;

public sealed record GetAllAppointmentsByDoctorIdQueryResponse(

    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string Text,
    Patient Patient

);