﻿using DB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LogsPersData> LogsPersData { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Flags> Flags { get; set; }
        public DbSet<PersData> PersData { get; set; }
        public DbSet<PersDataDocument> PersDataDocument { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
    [Table(name: "Logs", Schema = "dbo")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public int IdPU { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public DateTime? DateTime { get; set; }
    }
    public class Flags
    {
        [Key]
        public int Id { get; set; }
        public string NameAction { get; set; }
        public bool Flag { get; set; }
    }
}