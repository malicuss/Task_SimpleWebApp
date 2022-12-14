using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleWebApp.Core.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductId = 0;
        }
        
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please provide name of product")]
        [MinLength(5)]
        [MaxLength(40)]
        public string ProductName { get; set; } = null!;
        [Required(ErrorMessage = "Please chose Supplier from existing options")]
        public int? SupplierId { get; set; }
        [Required(ErrorMessage = "Please chose Category from existing options")]
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        #region NotMapped
        
        [NotMapped]
        public List<SelectListItem> Categories { get; set; } = new();
        [NotMapped]
        public List<SelectListItem> Suppliers { get; set; } = new();

        public void UpdateProduct(Product newProd)
        {
            ProductName = newProd.ProductName;
            QuantityPerUnit = newProd.QuantityPerUnit;
            UnitPrice = newProd.UnitPrice;
            UnitsInStock = newProd.UnitsInStock;
            UnitsOnOrder = newProd.UnitsOnOrder;
            ReorderLevel = newProd.ReorderLevel;
            Discontinued = newProd.Discontinued;
            SupplierId = newProd.SupplierId;
            CategoryId = newProd.CategoryId;
        }

        public override string ToString()
        {
            var res = string.Empty;

            res = $"{nameof(ProductId)} : {ProductId}\n" +
                  $"{nameof(ProductName)} : {ProductName}\n" +
                  $"{nameof(SupplierId)} : {SupplierId}\n" +
                  $"{nameof(CategoryId)} : {CategoryId}\n" +
                  $"{nameof(QuantityPerUnit)} : {QuantityPerUnit}\n" +
                  $"{nameof(UnitPrice)} : {UnitPrice}\n" +
                  $"{nameof(UnitsInStock)} : {UnitsInStock}\n" +
                  $"{nameof(UnitsOnOrder)} : {UnitsOnOrder}\n" +
                  $"{nameof(ReorderLevel)} : {ReorderLevel}\n" +
                  $"{nameof(Discontinued)} : {Discontinued}\n" +
                  $"{nameof(Category)} : {Category}\n" +
                  $"{nameof(Supplier)} : {Supplier}\n" +
                  $"{nameof(OrderDetails)} : {OrderDetails}\n";
            return res;
        }

        #endregion
    }
}
