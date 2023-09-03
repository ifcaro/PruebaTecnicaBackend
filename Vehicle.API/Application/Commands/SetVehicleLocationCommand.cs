using MediatR;

namespace Vehicle.API.Application.Commands
{
    public class SetVehicleLocationCommand : IRequest<bool>
    {
        public Guid VehicleId { get; private set; }
        public string Location { get; private set; }

        public SetVehicleLocationCommand(Guid vehicleId, string location)
        {
            VehicleId = vehicleId;
            Location = location;
        }
    }
}
