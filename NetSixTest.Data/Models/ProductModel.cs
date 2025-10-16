using NetSixTest.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetSixTest.Data.Models
{
    public class ProductModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Enabled { get; set; }

        public IList<ProductCategoryModel>? ProductsCategories { get; set; } = null;
    }

    public class ProductInsertModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Enabled { get; set; } 
        public IList<InsertProductPictureModel>? ProductPictures { get; set; } = null;
        public IList<ProductCategoryModel>? ProductsCategories { get; set; } = null;
    }
}
