using Vehicle.Domain.SeedWork;

namespace Vehicle.Domain.AggregatesModel.VehicleAggregate
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle> GetAsync(Guid vehicleId);

        Task<Vehicle> GetByOrderIdAsync(Guid orderId);

        Vehicle Add(Vehicle vehicle);
    }
}
