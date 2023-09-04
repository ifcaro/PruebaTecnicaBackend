using Microsoft.EntityFrameworkCore;
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

            await LoadChildColectionsAsync(vehicle);

            return vehicle!;
        }

        public async Task<Domain.AggregatesModel.VehicleAggregate.Vehicle> GetByOrderIdAsync(Guid orderId)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Orders.Any(y => y.OrderId == orderId && y.DateRemoved == null));

            await LoadChildColectionsAsync(vehicle);

            return vehicle!;
        }

        public Domain.AggregatesModel.VehicleAggregate.Vehicle Add(Domain.AggregatesModel.VehicleAggregate.Vehicle vehicle)
        {
            return _context.Vehicles.Add(vehicle).Entity;
        }

        private async Task LoadChildColectionsAsync(Domain.AggregatesModel.VehicleAggregate.Vehicle? vehicle)
        {
            if (vehicle! != null!)
            {
                await _context.Entry(vehicle)
                    .Collection(i => i.LocationHistory).LoadAsync();
                await _context.Entry(vehicle)
                    .Collection(i => i.Orders).LoadAsync();
            }
        }
    }
}
