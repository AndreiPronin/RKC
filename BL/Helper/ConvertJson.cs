using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public class ConvertJson<T> where T : class,new()
    {
        private T Value { get; set; }
        public ConvertJson(T value)
        {
            Value = value;
        }
        public ConvertJson()
        {

        }

        public string ConverModelToJson() 
        {
            var jsonString = JsonConvert.SerializeObject(Value);
            return jsonString;
        }
        public T ConverJsonToModel(string Json)
        {
            var Model = JsonConvert.DeserializeObject<T>(Json);
            return Model;
        }
    }
}
