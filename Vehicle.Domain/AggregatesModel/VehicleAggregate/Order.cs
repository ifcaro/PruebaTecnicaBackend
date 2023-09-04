using Vehicle.Domain.SeedWork;

namespace Vehicle.Domain.AggregatesModel.VehicleAggregate
{
    public class Order : Entity
    {
        public Guid OrderId { get; private set; }
        public DateTime DateAdded { get; private set; }
        public DateTime? DateRemoved { get; private set; }

        protected Order() { }

        public Order(Guid orderId, DateTime dateAdded)
        {
            OrderId = orderId;
            DateAdded = dateAdded;
        }

        public void SetDateRemoved(DateTime dateRemoved)
        {
            DateRemoved = dateRemoved;
        }
    }
}
