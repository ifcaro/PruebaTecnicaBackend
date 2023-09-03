using Vehicle.Domain.AggregatesModel.VehicleAggregate;
using Vehicle.Domain.SeedWork;

namespace Vehicle.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public VehicleRepository(VehicleContext context)
        {
            _context = context;
        }

        public async Task<Domain.AggregatesModel.VehicleAggregate.Vehicle> GetAsync(Guid vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            return vehicle!;
        }

        public Domain.AggregatesModel.VehicleAggregate.Vehicle Add(Domain.AggregatesModel.VehicleAggregate.Vehicle vehicle)
        {
            return _context.Vehicles.Add(vehicle).Entity;
        }
    }
}
