using MediatR;

namespace Vehicle.API.Application.Commands
{
    public class NotifyVehicleLocationChangedCommand : IRequest<bool>
    {
        public Guid VehicleId { get; private set; }
        public string CurrentLocation { get; private set; }

        public NotifyVehicleLocationChangedCommand(Domain.AggregatesModel.VehicleAggregate.Vehicle vehicle)
        {
            VehicleId = vehicle.Id;
            CurrentLocation = vehicle.CurrentLocation!;
        }
    }
}
