using MediatR;
using Moq;
using Vehicle.API.Application.Commands;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;
using Xunit;

namespace Vehicle.Tests.Application
{
    public class OrderCommandHandlerTest
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IMediator> _mediator;

        public OrderCommandHandlerTest()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Add_Order_return_false_if_vehicle_does_not_exist()
        {
            //Arrange   
            var fakeOrderCmd = FakeAddOrderCommand(new Dictionary<string, object>
            {
                ["vehicle"] = Guid.NewGuid(),
                ["orderId"] = Guid.NewGuid()
            });

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle)null!));

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new AddOrderCommandHandler(_vehicleRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeOrderCmd, cltToken);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Add_Order_return_false_if_order_does_exist()
        {
            //Arrange   
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var fakeOrderCmd = FakeAddOrderCommand(new Dictionary<string, object>
            {
                ["vehicle"] = vehicleId,
                ["orderId"] = orderId
            });

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(FakeVehicleWithOrder(vehicleId, orderId)));

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new AddOrderCommandHandler(_vehicleRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeOrderCmd, cltToken);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Add_Order_return_false_if_order_does_exist_in_other_vehicle()
        {
            //Arrange   
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var fakeOrderCmd = FakeAddOrderCommand(new Dictionary<string, object>
            {
                ["vehicle"] = vehicleId,
                ["orderId"] = orderId
            });

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.GetAsync(It.IsAny<Guid>()))
                .Returns((Guid x) => Task.FromResult(FakeVehicle(x)));

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.GetByOrderIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(FakeVehicleWithOrder(Guid.NewGuid(), orderId)));

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new AddOrderCommandHandler(_vehicleRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeOrderCmd, cltToken);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Remove_Order_return_false_if_vehicle_does_not_exist()
        {
            //Arrange   
            var fakeOrderCmd = FakeRemoveOrderCommand(new Dictionary<string, object>
            {
                ["vehicle"] = Guid.NewGuid(),
                ["orderId"] = Guid.NewGuid()
            });

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle)null!));

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new RemoveOrderCommandHandler(_vehicleRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeOrderCmd, cltToken);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Remove_Order_return_false_if_order_does_not_exist()
        {
            //Arrange   
            var vehicleId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var fakeOrderCmd = FakeRemoveOrderCommand(new Dictionary<string, object>
            {
                ["vehicle"] = vehicleId,
                ["orderId"] = orderId
            });

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(FakeVehicle(vehicleId)));

            _vehicleRepositoryMock.Setup(vehicleRepository => vehicleRepository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new RemoveOrderCommandHandler(_vehicleRepositoryMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeOrderCmd, cltToken);

            //Assert
            Assert.False(result);
        }

        private Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle FakeVehicle(Guid vehicleId)
        {
            return new Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle(vehicleId);
        }

        private Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle FakeVehicleWithOrder(Guid vehicleId, Guid orderId)
        {
            var vehicle = new Vehicle.Domain.AggregatesModel.VehicleAggregate.Vehicle(vehicleId);

            vehicle.AddOrder(orderId);

            return vehicle;
        }

        private AddOrderCommand FakeAddOrderCommand(Dictionary<string, object> args = null!)
        {
            return new AddOrderCommand(
                vehicle: args != null && args.ContainsKey("vehicle") ? (Guid)args["vehicle"] : Guid.Empty,
                orderId: args != null && args.ContainsKey("orderId") ? (Guid)args["orderId"] : Guid.Empty);
        }

        private RemoveOrderCommand FakeRemoveOrderCommand(Dictionary<string, object> args = null!)
        {
            return new RemoveOrderCommand(
                vehicle: args != null && args.ContainsKey("vehicle") ? (Guid)args["vehicle"] : Guid.Empty,
                orderId: args != null && args.ContainsKey("orderId") ? (Guid)args["orderId"] : Guid.Empty);
        }
    }
}
