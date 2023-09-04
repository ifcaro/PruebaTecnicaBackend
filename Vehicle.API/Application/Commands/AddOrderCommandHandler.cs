using MediatR;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public AddOrderCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> Handle(AddOrderCommand command, CancellationToken cancellationToken)
        {
            var vehicleToUpdate = await _vehicleRepository.GetAsync(command.VehicleId);

            if (vehicleToUpdate == null!)
            {
                return false;
            }

            if (vehicleToUpdate.Orders.Any(x => x.OrderId == command.OrderId && x.DateRemoved == null))
            {
                return false;
            }

            var anotherVehicleWithOrder = await _vehicleRepository.GetByOrderIdAsync(command.OrderId);

            if(anotherVehicleWithOrder != null!)
            {
                return false;
            }

            vehicleToUpdate.AddOrder(command.OrderId);

            return await _vehicleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
