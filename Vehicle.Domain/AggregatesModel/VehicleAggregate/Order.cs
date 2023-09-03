using Vehicle.Domain.SeedWork;

namespace Vehicle.Domain.AggregatesModel.VehicleAggregate
{
    public class Order : Entity
    {
        protected Order() { }

        public Order(Guid orderId)
        {
            Id = orderId;
        }
    }
}
