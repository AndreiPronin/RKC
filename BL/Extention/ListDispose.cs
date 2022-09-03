
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class ListDispose
    {
        public static void Dispose<T>(this IEnumerable<T> source)
        {
            foreach (IDisposable disposableObject in source)
            {
                disposableObject.Dispose();
            };
        }

    }
    
}
