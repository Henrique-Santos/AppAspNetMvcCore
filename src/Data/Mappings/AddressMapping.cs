using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PublicPlace)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasColumnType("varchar(8)");

            builder.Property(x => x.Complement)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(x => x.Neighborhood)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Addresses");
        }
    }
}