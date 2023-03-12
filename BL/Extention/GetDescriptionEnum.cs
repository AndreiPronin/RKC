using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class GetDescriptionEnum
    {
		public static string GetDescription(this Enum enumElement)
		{
			Type type = enumElement.GetType();

			MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
			if (memInfo != null && memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attrs != null && attrs.Length > 0)
					return ((DescriptionAttribute)attrs[0]).Description;
			}

			return enumElement.ToString();
		}
        public static string GetDescription(this Enum enumElement, int? Values)
        {
            Type type = enumElement.GetType();

            MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return $"{enumElement.ToString()} {Values.Value}";
        }
    }
}
