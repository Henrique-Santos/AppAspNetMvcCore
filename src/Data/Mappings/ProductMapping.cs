using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    // Mapeando os objetos do banco usando Fluent Api
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id); // A chave do Produto é o Id

            builder.Property(x => x.Name) // Selecionando o campo Name
                .IsRequired() // É um campo obrigatório
                .HasColumnType("varchar(200)"); // O tipo dessa coluna é varchar(200)

            builder.Property(x => x.Description)
               .IsRequired()
               .HasColumnType("varchar(1000)");

            builder.Property(x => x.Image)
               .IsRequired()
               .HasColumnType("varchar(100)");

            builder.ToTable("Products"); // Definindo o nome da tabela. (Como segundo parametro é possivel passar o nome do schema "o padrão é: dbo")
        }
    }
}