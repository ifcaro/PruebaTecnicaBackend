using MediatR;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class SetVehicleLocationCommandHandler : IRequestHandler<SetVehicleLocationCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IVehicleRepository _vehicleRepository;

        public SetVehicleLocationCommandHandler(
            IMediator mediator, 
            IVehicleRepository vehicleRepository)
        {
            _mediator = mediator;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> Handle(SetVehicleLocationCommand command, CancellationToken cancellationToken)
        {
            var vehicleToUpdate = await _vehicleRepository.GetAsync(command.VehicleId);

            if (vehicleToUpdate == null!)
            {
                vehicleToUpdate = new Domain.AggregatesModel.VehicleAggregate.Vehicle(command.VehicleId);
                _vehicleRepository.Add(vehicleToUpdate);
            }

            vehicleToUpdate.SetCurrentLocation(command.Location);

            var result = await _vehicleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if (result)
            {
                await _mediator.Send(new NotifyVehicleLocationChangedCommand(vehicleToUpdate), cancellationToken);
            }

            return result;
        }
    }
}
