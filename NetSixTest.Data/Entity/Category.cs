using NetSixTest.Data.Interfaces;
using NetSixTest.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetSixTest.Data.Entity
{
    public class Category : IEntity
    {
        public Category()
        {


        }

        public Category(CategoryModel model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Enabled= model.Enabled;
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
    }
}
