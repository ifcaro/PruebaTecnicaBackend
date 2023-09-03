using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Vehicle.Infrastructure.EntityConfigurations
{
    class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.VehicleAggregate.Vehicle>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.VehicleAggregate.Vehicle> builder)
        {
            builder.ToTable("Vehicle");

            builder.HasKey(x => x.Id);

            builder.Ignore(b => b.DomainEvents);

            builder
            .HasMany(e => e.LocationHistory)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

            builder
            .HasMany(e => e.Orders)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
