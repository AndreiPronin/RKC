using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Query
{
    public static class QueryPers
    {
        public static string GtPersDublicate = $@"select A.Lic as FULL_LIC from [WEB_APP].[dbo].[PersData] A join (
	select *, ROW_NUMBER()over (PARTITION BY Lic order by Lic desc) as RN from [WEB_APP].[dbo].[PersData]
	where Main = 1 and IsDelete = 0
) b on b.Lic = A.Lic and b.IsDelete = A.IsDelete
where b.RN >1";
	}
}
