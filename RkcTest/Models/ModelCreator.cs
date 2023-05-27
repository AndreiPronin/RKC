using BE.Counter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RkcTest.Models
{
    public class ModelCreator
    {
        public static ModelAddPU  modelAddPU = new ModelAddPU() { 
             DATE_CHECK = DateTime.Now,
        };
    }
}
