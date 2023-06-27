namespace Business.Models
{
    public class Product : Entity
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Value { get; set; }
        public DateTime DateRegistration { get; set; }

        // EF Relation
        public Supplier Supplier { get; set; }
    }
}