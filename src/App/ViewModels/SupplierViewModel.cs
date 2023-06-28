using Business.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        public string Document { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        [DisplayName("Type")]
        public int SupplierType { get; set; }

        public AddressViewModel Address { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }


        //var supplierViewModel = (SupplierViewModel)new Supplier();
        public static explicit operator SupplierViewModel(Supplier supplier)
        {
            return new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Document = supplier.Document,
                Active = supplier.Active,
                SupplierType = (int)supplier.SupplierType,
                Address = (AddressViewModel)supplier.Address,
                Products = (IEnumerable<ProductViewModel>)supplier.Products
            };
        }
    }
}