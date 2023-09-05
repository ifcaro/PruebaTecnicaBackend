using MediatR;
using Vehicle.API.Application.Models;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class RemoveOrderCommandHandler : IRequestHandler<RemoveOrderCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IVehicleRepository _vehicleRepository;

        public RemoveOrderCommandHandler(
            IMediator mediator, 
            IVehicleRepository vehicleRepository)
        {
            _mediator = mediator;
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

            var order = vehicleToUpdate.RemoveOrder(command.OrderId);

            var result = await _vehicleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if (result && order! != null!)
            {
                await _mediator.Send(new NotifyOrderRemovedCommand(command.VehicleId, order), cancellationToken);
            }

            return result;
        }
    }
}
