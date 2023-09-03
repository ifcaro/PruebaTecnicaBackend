using Vehicle.Domain.SeedWork;

namespace Vehicle.Domain.AggregatesModel.VehicleAggregate
{
    public class Vehicle : Entity, IAggregateRoot
    {
        public string? CurrentLocation { get; private set; }
        public List<Location> LocationHistory { get; private set; }
        public List<Order> Orders { get; private set; }

        protected Vehicle()
        {
            LocationHistory = new List<Location>();
            Orders = new List<Order>();
        }

        public Vehicle(Guid idVehicle) : this()
        {
            Id = idVehicle;
        }

        public void SetCurrentLocation(string location)
        {
            CurrentLocation = location;
            LocationHistory.Add(new Location(DateTime.UtcNow, location));
        }

        public void AddOrder(Guid orderId)
        {
            Orders.Add(new Order(orderId));
        }
    }
}
