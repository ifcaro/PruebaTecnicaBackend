namespace Vehicle.API.Application.Queries
{
    public record Vehicle
    {
        public Guid VehicleId { get; init; }
        public string? CurrentLocation { get; init; }
    }

    public record Location
    {
        public DateTime Date { get; init; }
        public string Coordinates { get; init; } = default!;
    }

    public record VehicleLocationHistory
    {
        public Guid VehicleId { get; init; }

        public List<Location> LocationHistory { get; init; } = default!;
    }
}
