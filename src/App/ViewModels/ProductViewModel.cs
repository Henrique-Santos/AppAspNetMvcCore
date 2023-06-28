using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Supplier")]
        public Guid SupplierId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Value { get; set; }

        [ScaffoldColumn(false)] // Irá desconsiderar essa propriedade ao fazer o scaffolding
        public DateTime DateRegistration { get; set; }

        public string Image { get; set; }

        [DisplayName("Product Image")]
        public IFormFile ImageUpload { get; set; }

        public SupplierViewModel Supplier { get; set; }

        public static explicit operator ProductViewModel(Product product)
        {
            return new ProductViewModel
            {
               Id = product.Id,
               SupplierId = product.SupplierId,
               Name = product.Name,
               Description = product.Description,
               Value = product.Value,
               DateRegistration = product.DateRegistration,
               Image = product.Image,
               Supplier = (SupplierViewModel)product.Supplier
            };
        }
    }
}