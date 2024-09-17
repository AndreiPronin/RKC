using System;

namespace DB.Model
{
	public class ManualRecalculationsByFullLic
	{
		public int ServiceId { get; set; }
		public Guid Guid { get; set; }
		public string RecalculationRange { get; set; }
		public string ServiceName { get; set; }
		public string RecalculationReason { get; set; }
		public decimal RecalculationValue { get; set; }
		public string RecalculationOwner { get; set; }
		public string Comment { get; set; }
	}
}
