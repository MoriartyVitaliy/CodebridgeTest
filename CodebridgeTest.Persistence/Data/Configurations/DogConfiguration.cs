using CodebridgeTest.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodebridgeTest.Persistence.Data.Configurations
{
    public class DogConfiguration : IEntityTypeConfiguration<Dog>
    {
        public void Configure(EntityTypeBuilder<Dog> builder)
        {
            // Table name
            builder.ToTable("dogs");

            // Primary key — `name`
            builder.HasKey(x => x.Name);
            

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Color)
                .HasColumnName("color")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.TailLength)
                .HasColumnName("tail_length")
                .IsRequired();

            builder.Property(x => x.Weight)
                .HasColumnName("weight")
                .IsRequired();
        }
    }
}
