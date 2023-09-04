namespace Vehicle.API.Application.Queries
{
    public interface IVehicleQueries
    {
        Task<IEnumerable<Vehicle>> GetAllVehicleLocationAsync();
        Task<Vehicle> GetVehicleLocationAsync(Guid vehicleId);
        Task<VehicleLocationHistory> GetVehicleLocationHistoryAsync(Guid vehicleId);
        Task<VehicleOrders> GetVehicleOrdersAsync(Guid vehicleId);
        Task<Vehicle> GetVehicleByOrderAsync(Guid orderId);
    }
}
