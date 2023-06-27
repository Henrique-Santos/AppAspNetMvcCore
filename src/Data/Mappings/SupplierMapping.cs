using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class SupplierMapping : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Document)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 | Supplier -> Address
            builder.HasOne(x => x.Address) // Definindo um relacionamento de 1 para 1. Onde 1 Fornecedor tem um Endereço (HasOne) e o Endereço tem um Fornecedor (WithOne)
                .WithOne(x => x.Supplier);

            // 1 : N | Supplier -> Products
            builder.HasMany(x => x.Products) // Definindo um relacionamento de 1 para Muitos. Onde 1 Fornecedor tem muitos Produtos (HasMany) e o Produto tem um Fornecedor (WithOne) e o campo que faz essa ligaçáo é o SupplierId (HasForeignKey)
                .WithOne(x => x.Supplier)
                .HasForeignKey(x => x.SupplierId);

            builder.ToTable("Suppliers");
        }
    }
}