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
        public int CategoryId { get; set; }
    }

    public class ProductInsertModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Enabled { get; set; }
        public int CategoryId { get; set; }
        public IList<ProductPicture>? ProductPictures { get; set; } = null;
    }
}
