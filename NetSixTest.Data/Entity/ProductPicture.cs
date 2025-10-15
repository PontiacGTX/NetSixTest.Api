using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetSixTest.Data.Entity
{
    public class ProductPicture
    {
        [Key]
        public Guid? ProductPictureId { get; set; }
        public byte[] PictureData { get; set; }
        public string FileName { get; set; }
        public string? Hash { get; set; }
         

        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }


    public class InsertProductPictureModel
    {
        public string PictureData { get; set; }
        public string FileName { get; set; }  
        public int? ProductId { get; set; } 
    }
}
