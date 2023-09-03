using Vehicle.Domain.SeedWork;

namespace Vehicle.Domain.AggregatesModel.VehicleAggregate
{
    public class Location : Entity
    {
        public DateTime Date { get; private set; }
        public string Coordinates { get; private set; } = default!;

        protected Location() { }

        public Location(DateTime date, string coordinates)
        {
            Date = date;
            Coordinates = coordinates;
        }
    }
}
