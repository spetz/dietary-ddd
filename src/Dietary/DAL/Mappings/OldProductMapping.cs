using Dietary.Models.NewProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dietary.DAL.Mappings
{
    public class OldProductMapping : IEntityTypeConfiguration<OldProduct>
    {
        public void Configure(EntityTypeBuilder<OldProduct> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}