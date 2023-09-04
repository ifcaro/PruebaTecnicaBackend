using MediatR;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class RemoveOrderCommandHandler : IRequestHandler<RemoveOrderCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public RemoveOrderCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> Handle(RemoveOrderCommand command, CancellationToken cancellationToken)
        {
            var vehicleToUpdate = await _vehicleRepository.GetAsync(command.VehicleId);

            if (vehicleToUpdate == null!)
            {
                return false;
            }

            if(!vehicleToUpdate.Orders.Any(x => x.OrderId == command.OrderId && x.DateRemoved == null))
            {
                return false;
            }

            vehicleToUpdate.RemoveOrder(command.OrderId);

            return await _vehicleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
