using MediatR;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class NotifyOrderRemovedCommand : IRequest<bool>
    {
        public Guid VehicleId { get; private set; }
        public Guid OrderId { get; private set; }
        public DateTime DateAdded { get; private set; }
        public DateTime DateRemoved { get; private set; }

        public NotifyOrderRemovedCommand(Guid vehicleId, Order order)
        {
            VehicleId = vehicleId;
            OrderId = order.OrderId;
            DateAdded = order.DateAdded;
            DateRemoved = order.DateRemoved!.Value;
        }
    }
}
