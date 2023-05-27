using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public class ThrowHelper<T>
    {
        public static T ThrowIsNull(T value)
        {
            if (value == null) throw new ArgumentNullException($"{nameof(T)} is null"); 
            return value;
        }
    }
}
