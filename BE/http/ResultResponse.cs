using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.http
{
    public class ResultResponse<T1,T2> 
        where T1 : class
        where T2 : class, new()
    {
        public T1 lastId { get; set; }
        public T2 value { get; set; }
        public ResultResponse()
        {
            if (typeof(T2).IsClass)
                value = new T2();
        }
    }
}
