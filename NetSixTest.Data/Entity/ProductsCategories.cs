using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Data.Entity
{
    public class ProductsCategories
    {
        [Key]
        public Guid? Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }


    public class ProductsCategoriesModel : ProductsCategories
    { }
}
