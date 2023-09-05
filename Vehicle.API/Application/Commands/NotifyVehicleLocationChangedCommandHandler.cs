using MediatR;
using Microsoft.AspNetCore.SignalR;
using Vehicle.API.RealTime;

namespace Vehicle.API.Application.Commands
{
    public class NotifyVehicleLocationChangedCommandHandler : IRequestHandler<NotifyVehicleLocationChangedCommand, bool>
    {
        private readonly IHubContext<VehicleEventsClientHub> _hubContext;

        public NotifyVehicleLocationChangedCommandHandler(IHubContext<VehicleEventsClientHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(NotifyVehicleLocationChangedCommand command, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("locationChanged", command, cancellationToken);

            return true;
        }
    }
}
