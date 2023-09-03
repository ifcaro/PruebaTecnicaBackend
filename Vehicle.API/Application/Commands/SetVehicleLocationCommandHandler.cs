using MediatR;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class SetVehicleLocationCommandHandler : IRequestHandler<SetVehicleLocationCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public SetVehicleLocationCommandHandler(IVehicleRepository vehicleRepository)
        {
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

            return await _vehicleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
