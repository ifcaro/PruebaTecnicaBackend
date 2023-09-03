using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Vehicle.Infrastructure.EntityConfigurations
{
    class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.VehicleAggregate.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.VehicleAggregate.Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.Id);

            builder.Ignore(b => b.DomainEvents);
        }
    }
}
