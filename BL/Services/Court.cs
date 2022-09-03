using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICourt
    {
        string DetailInfroms(string FULL_LIC);
    }
    public class Court : ICourt
    {
        public string DetailInfroms(string FULL_LIC)
        {
            throw new NotImplementedException();
        }
    }
}
