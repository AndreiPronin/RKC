using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ApiServices
{
    public interface IApiCounters
    {
        Task GetIpuReadingsForGis();
    }
    public class ApiCounters : IApiCounters
    {
        public async Task GetIpuReadingsForGis()
        {
            throw new NotImplementedException();
        }
    }
}
