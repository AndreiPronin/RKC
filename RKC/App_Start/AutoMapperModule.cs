using AutoMapper;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RKC.App_Start
{
    public class AutoMapperModule
    {
        public void RegisterServices(IKernel kernel)
        {
            var mapperConfiguration = CreateConfiguration();
            kernel.Bind<MapperConfiguration>().ToConstant(mapperConfiguration).InSingletonScope();

            // This teaches Ninject how to create automapper instances say if for instance
            // MyResolver has a constructor with a parameter that needs to be injected
            kernel.Bind<IMapper>().ToMethod(ctx =>
                 new Mapper(mapperConfiguration, type => ctx.Kernel.Get(type)));
        }

        //public override void Load()
        //{
        //    //Bind<IValueResolver<SourceEntity, DestModel, bool>>().To<MyResolver>();

        //    var mapperConfiguration = CreateConfiguration();
        //    Bind<MapperConfiguration>().ToConstant(mapperConfiguration).InSingletonScope();

        //    // This teaches Ninject how to create automapper instances say if for instance
        //    // MyResolver has a constructor with a parameter that needs to be injected
        //    Bind<IMapper>().ToMethod(ctx =>
        //         new Mapper(mapperConfiguration, type => ctx.Kernel.Get(type)));
        //}

        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new[] { "RKC", "BL", "DB", "BE" });
                //cfg.AddProfiles(new[] { });
            });

            return config;
        }
    }
}