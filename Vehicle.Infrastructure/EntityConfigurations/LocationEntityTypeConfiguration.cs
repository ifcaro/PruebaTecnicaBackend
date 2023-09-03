using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Vehicle.Infrastructure.EntityConfigurations
{
    class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.VehicleAggregate.Location>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.VehicleAggregate.Location> builder)
        {
            builder.ToTable("Location");

            builder.HasKey(x => x.Id);

            builder.Ignore(b => b.DomainEvents);
        }
    }
}
