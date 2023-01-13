
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class ListDispose
    {
        public static void DisposeAll(this IEnumerable set)
        {
            foreach (Object obj in set)
            {
                IDisposable disp = obj as IDisposable;
                if (disp != null) { disp.Dispose(); }
            }
        }

    }
    
}
