using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excels
{
	class DataBase: DbContext
	{
		public DbSet<ZHR_EMP_FULL> dictionaris { get; set; }
		public DbSet<ZHR_SHT> emp { get; set; }

		public DataBase() : base("DefaultConnection")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ZHR_EMP_FULL>().HasKey(x => new { x.CP_ID });
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<ZHR_SHT>().HasKey(x => new { x.PLANS });
			base.OnModelCreating(modelBuilder);
		}
	}
    [Table("ZHR_EMP_FULL")]
    public class ZHR_EMP_FULL
	{
		public string CP_ID { get; set; }
		public string NACHN { get; set; }
		public string VORNA { get; set; }
		public string MIDNM { get; set; }
		public string STATUSA { get; set; }
		public string PLANS { get; set; }
		public string EMAIL { get; set; }
		public List<ZHR_SHT> emp { get; set; } = new List<ZHR_SHT>();
	}
	[Table("ZHR_SHT")]
	public class ZHR_SHT
	{
		public ZHR_EMP_FULL dictionaris { get; set; }
		public string PLANS { get; set; }
		public string PLANS_DESC { get; set; }
	}
}
