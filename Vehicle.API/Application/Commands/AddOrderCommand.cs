using MediatR;

namespace Vehicle.API.Application.Commands
{
    public class AddOrderCommand : IRequest<bool>
    {
        public Guid VehicleId { get; private set; }
        public Guid OrderId { get; private set; }

        public AddOrderCommand(Guid vehicle, Guid orderId)
        {
            VehicleId = vehicle;
            OrderId = orderId;
        }
    }
}
