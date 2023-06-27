using Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        // Método chamado durante a criação desse modelo no bd
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pega todas as propriedades do tipo string existentes nas entidades e define varchar(100)
            // * Só será aplicado em propriedadas não mapeadas. Evitando o uso do nvarchar(MAX) *
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties().Where(x => x.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties().Where(x => x.ClrType == typeof(decimal))))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // Pega todas as Entidades que estão mapeadas no contexto. Após isso busca todas as Classes que herdem de IEntityTypeConfguration que apontam para as entidades mapeadas e faz o registro dos mappings.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
            
            // Pega todas as relações do modelBuilder e desabilita a deleção em cascata
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys())) 
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder); 
        }
    }
}