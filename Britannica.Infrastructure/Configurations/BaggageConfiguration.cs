using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class BaggageConfiguration : IEntityTypeConfiguration<BaggageEntity>
    {
        public void Configure(EntityTypeBuilder<BaggageEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Weight).IsRequired();
        }
    }
}
