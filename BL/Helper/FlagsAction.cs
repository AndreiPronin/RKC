﻿using BE.Service;
using DB.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public interface IFlagsAction
    {
        bool GetAction(string Method);
        void Trigger(string Method);
        Flags GetFlag(EnumFlags enumFlags);
    }
    public class FlagsAction : IFlagsAction
    {
        public bool GetAction(string Method)
        {
            if (!string.IsNullOrEmpty(Method))
            {
                using (var db = new ApplicationDbContext())
                {
                    return db.Flags.Where(x => x.NameAction == Method).FirstOrDefault().Flag;
                }
            }
            else return false;
        }
        public Flags GetFlag(EnumFlags enumFlags)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Flags.FirstOrDefault(x => x.NameAction == nameof(EnumFlags.ReestyGPAccountingDepartment));
            }
        }
        public void Trigger(string Method)
        {
            using (var db = new ApplicationDbContext())
            {
                var Flag = db.Flags.Where(x => x.NameAction == Method).FirstOrDefault();
                Flag.Flag = !Flag.Flag;
                db.SaveChanges();
            }
        }
    }
}
