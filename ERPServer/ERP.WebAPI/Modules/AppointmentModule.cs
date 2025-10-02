using ERP.Application.Features.Appointments.CreatERP;
using ERP.Application.Features.Appointments.GetAllAppointmentsByDoctorId;
using ERP.Application.Features.Appointments.GetAllDoctorsByDepartment;
using ERP.Application.Features.Appointments.GetPatientByIdentityNumber;
using ERPServer.Application.Features.Appointments.DeletERPById;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;
using TS.Result;

namespace ERP.WebAPI.Modules;

public static class AppointmentModule
{
    public static void RegisterAppointmentModuleRootes(this IEndpointRouteBuilder endpointRouteBuilder)
    {

        RouteGroupBuilder group = endpointRouteBuilder.MapGroup("/api/appointments").WithTags("Appointments")
                .RequireAuthorization();

        group.MapGet("/GetAllDoctorsByDepartment", async (
        [FromServices] ISender sender,
        [FromQuery] int departmentValue,
        CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(
                new GetAllDoctorsByDepartmentQuery(departmentValue),
                cancellationToken);

            return response.IsSuccessful
                ? Results.Ok(response)
                : Results.BadRequest(response);
        });

        group.MapGet("/GetAllAppointmentsByDoctorId", async (
        [FromServices] ISender sender,
        [FromQuery] Guid doctorId,
        CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(
            new GetAllAppointmentsByDoctorIdQuery(doctorId),
            cancellationToken);

            return response.IsSuccessful
            ? Results.Ok(response)
            : Results.BadRequest(response);
        });

        group.MapGet("/GetPatientByIdentityNumber", async (
        [FromServices] ISender sender,
        [FromQuery] string IdentityNumber,
        CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(
            new GetPatientByIdentityNumberQuery(IdentityNumber),
            cancellationToken);

            return response.IsSuccessful
            ? Results.Ok(response)
            : Results.BadRequest(response);
        });

        group.MapPost(
            string.Empty, async (
                ISender sender,
                [FromBody] CreatERPCommand request,
                CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapDelete("{id:guid}", async (
            [FromServices] ISender sender,
            Guid id,
            CancellationToken cancellationToken) =>
            {
                var command = new DeletERPByIdCommand(id);
                var response = await sender.Send(command, cancellationToken);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        // group.MapPut("{id:guid}", async (
        //     [FromServices] ISender sender,
        //     Guid id,
        //     DoctorUpdateCommand request,
        //     CancellationToken cancellationToken) =>
        //     {

        //         var command = request with { Id = id };

        //         var response = await sender.Send(command, cancellationToken);
        //         return response.IsSuccessful
        //             ? Results.Ok(response)
        //             : Results.InternalServerError(response);

        //     })
        //     .Produces<Result<string>>();

    }

}