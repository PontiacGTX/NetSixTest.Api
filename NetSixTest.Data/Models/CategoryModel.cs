using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetSixTest.Data.Models
{
    public class CategoryModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled  { get; set; }
    }
    public class CategoryInsertModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled  { get; set; }
    }
}
