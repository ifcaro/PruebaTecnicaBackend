using Vehicle.Domain.SeedWork;

namespace Vehicle.Domain.AggregatesModel.VehicleAggregate
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle> GetAsync(Guid vehicleId);

        Vehicle Add(Vehicle vehicle);
    }
}
