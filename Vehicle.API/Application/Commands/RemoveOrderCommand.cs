using MediatR;

namespace Vehicle.API.Application.Commands
{
    public class RemoveOrderCommand : IRequest<bool>
    {
        public Guid VehicleId { get; private set; }
        public Guid OrderId { get; private set; }

        public RemoveOrderCommand(Guid vehicle, Guid orderId)
        {
            VehicleId = vehicle;
            OrderId = orderId;
        }
    }
}
