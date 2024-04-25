using AppCache;
using BL.Services;
using Ninject;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Jobs
{
    public class CacheUpdate : IJob
    {
        private readonly IDpu _dpu;
        private readonly ICacheApp _caheApp;
        public CacheUpdate()
        {
            var kernel = new StandardKernel();
            Module.RegistrationService(kernel);
            _dpu = kernel.Get<IDpu>();
            _caheApp = kernel.Get<ICacheApp>();
        }
        public void Execute(IJobExecutionContext context)
        {
            _caheApp.Delete(nameof(_dpu.SearchAutocompleteDPU));
            _dpu.SearchAutocompleteDPU("123");
        }
    }
}
