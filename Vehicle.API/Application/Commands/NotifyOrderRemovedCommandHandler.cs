using MediatR;
using Microsoft.AspNetCore.SignalR;
using Vehicle.API.RealTime;

namespace Vehicle.API.Application.Commands
{
    public class NotifyOrderRemovedCommandHandler : IRequestHandler<NotifyOrderRemovedCommand, bool>
    {
        private readonly IHubContext<VehicleEventsClientHub> _hubContext;

        public NotifyOrderRemovedCommandHandler(IHubContext<VehicleEventsClientHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(NotifyOrderRemovedCommand command, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("orderRemoved", command, cancellationToken);

            return true;
        }
    }
}
