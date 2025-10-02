using ERP.Domain.Entities;
using ERP.Domain.Repositories;
using ERP.Infrastructure.Context;
using GenericRepository;

namespace ERP.Infrastructure.Repositories;

public sealed class AppointmentRepository(ApplicationDbContext context) : Repository<Appointment, ApplicationDbContext>(context), IAppointmentRepository
{
}