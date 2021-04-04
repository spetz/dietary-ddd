using Dietary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dietary.DAL.Mappings
{
    public class CustomerOrderGroupMapping : IEntityTypeConfiguration<CustomerOrderGroup>
    {
        public void Configure(EntityTypeBuilder<CustomerOrderGroup> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Customer)
                .WithOne(x => x.Group)
                .HasForeignKey<CustomerOrderGroup>(x => x.CustomerId);
            builder.HasOne(x => x.Parent);
            builder.HasMany(x => x.Children);
            builder.HasMany(x => x.Orders);
        }
    }
}