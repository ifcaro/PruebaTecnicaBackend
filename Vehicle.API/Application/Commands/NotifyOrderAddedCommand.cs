using MediatR;
using Vehicle.Domain.AggregatesModel.VehicleAggregate;

namespace Vehicle.API.Application.Commands
{
    public class NotifyOrderAddedCommand : IRequest<bool>
    {
        public Guid VehicleId { get; private set; }
        public Guid OrderId { get; private set; }
        public DateTime DateAdded { get; private set; }

        public NotifyOrderAddedCommand(Guid vehicleId, Order order)
        {
            VehicleId = vehicleId;
            OrderId = order.OrderId;
            DateAdded = order.DateAdded;
        }
    }
}
