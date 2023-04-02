using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RKC.Extensions
{
    public class CustomModelBinders
    {
        public class CustomDecimalBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                return value.ConvertTo(typeof(decimal), new System.Globalization.CultureInfo("us-US"));
            }
        }
        public class CustomNullDoubleBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                return value.ConvertTo(typeof(double?), new System.Globalization.CultureInfo("us-US"));
            }
        }
        public class CustomNullDecimalBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                return value.ConvertTo(typeof(decimal?), new System.Globalization.CultureInfo("us-US"));
            }
        }
    }

    public static class CustomModelBindersConfig
    {
        public static void RegisterCustomModelBinders()
        {
            ModelBinders.Binders.Add(typeof(decimal), new CustomModelBinders.CustomDecimalBinder());
            ModelBinders.Binders.Add(typeof(double?), new CustomModelBinders.CustomNullDoubleBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new CustomModelBinders.CustomNullDecimalBinder());
        }
    }
}