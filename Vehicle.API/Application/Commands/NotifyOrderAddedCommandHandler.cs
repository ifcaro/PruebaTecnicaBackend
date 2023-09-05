using MediatR;
using Microsoft.AspNetCore.SignalR;
using Vehicle.API.RealTime;

namespace Vehicle.API.Application.Commands
{
    public class NotifyOrderAddedCommandHandler : IRequestHandler<NotifyOrderAddedCommand, bool>
    {
        private readonly IHubContext<VehicleEventsClientHub> _hubContext;

        public NotifyOrderAddedCommandHandler(IHubContext<VehicleEventsClientHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(NotifyOrderAddedCommand command, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("orderAdded", command, cancellationToken);

            return true;
        }
    }
}
