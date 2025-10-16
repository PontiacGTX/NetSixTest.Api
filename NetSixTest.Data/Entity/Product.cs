using NetSixTest.Data.Interfaces;
using NetSixTest.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Data.Entity;

public class Product : IEntity
{
    public Product()
    {

    }
    public Product(ProductModel model)
    {
        this.Enabled = model.Enabled; 
        this.Name = model.Name;
        this.Price = model.Price; 
        this.Quantity = model.Quantity;
        this.Id = model.Id;
    }
    public Product(ProductInsertModel model)
    {
        this.Enabled = model.Enabled; 
        this.Name = model.Name;
        this.Price = model.Price; 
        this.Quantity = model.Quantity;
    }
    [Key]
    public int Id { get ; set ; }

    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool Enabled { get; set; } 
    public ICollection<ProductPicture>? Pictures { get; set; }
    public ICollection<ProductsCategories>? ProductsCategories { get; set; }
}
