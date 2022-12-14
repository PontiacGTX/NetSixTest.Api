using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Data.Interfaces;

public interface IPropertyFieldHolder<T> where T : class
{
    T Field { get; set; }
}
