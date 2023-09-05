using Xunit;

namespace Vehicle.Tests.Domain
{
    public class VehicleAggregateTest
    {
        [Fact]
        public void Add_vehicle_location_sets_current_location_and_history()
        {
            //Arrange    
            var vehicleId = Guid.NewGuid();
            var location = "test_location";

            //Act 
            var fakeVehicle = new Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle(Guid.NewGuid());
            fakeVehicle.SetCurrentLocation(location);

            //Assert
            Assert.Equal(fakeVehicle.CurrentLocation, location);
            Assert.Contains(fakeVehicle.LocationHistory, x => x.Coordinates == location);
        }

        [Fact]
        public void Add_order_sets_date_added()
        {
            //Arrange    
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            //Act 
            var fakeVehicle = new Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle(Guid.NewGuid());
            fakeVehicle.AddOrder(orderId);

            //Assert
            Assert.Contains(fakeVehicle.Orders, x => x.OrderId == orderId && x.DateAdded != default && x.DateRemoved == null);
        }

        [Fact]
        public void Remove_order_sets_date_removed()
        {
            //Arrange    
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            //Act 
            var fakeVehicle = new Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle(Guid.NewGuid());
            fakeVehicle.AddOrder(orderId);
            fakeVehicle.RemoveOrder(orderId);

            //Assert
            Assert.Contains(fakeVehicle.Orders, x => x.OrderId == orderId && x.DateAdded != default && x.DateRemoved != null);
        }
    }
}
