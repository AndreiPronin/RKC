
namespace BL.Query
{
    public static class QueryCheckDublicate
    {
        public static string GVS1 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ГВС1%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
        public static string GVS2 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ГВС2%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
        public static string GVS3 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ГВС3%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
		public static string GVS4 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ГВС4%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
		public static string OTP1 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ОТП1%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
		public static string OTP2 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ОТП2%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
		public static string OTP3 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ОТП3%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
		public static string OTP4 = $@" select A.FULL_LIC as FULL_LIC ,A.TYPE_PU as TYPE_PU  from [IPU].[IPU_COUNTERS] A join (
	select *, ROW_NUMBER()over (PARTITION BY FULL_LIC order by FULL_LIC desc) as RN from [IPU].[IPU_COUNTERS]
	where CLOSE_ is null and TYPE_PU like '%ОТП4%'
) b on b.FULL_LIC = A.FULL_LIC and b.TYPE_PU = A.TYPE_PU
where b.RN >1";
	}
}
