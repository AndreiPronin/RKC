using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Roles
{
    public static class RolesEnums
    {
        [Description("Admin")]
        public const string Admin = "Admin";
        [Description("DPUReader")]
        public const string DPUReader = "DPUReader";
        [Description("DPUEdit")]
        public const string DPUEdit = "DPUEdit";
        [Description("DPUAdmin")]
        public const string DPUAdmin = "DPUAdmin";
        [Description("User")]
        public const string User = "User";
        [Description("PersReader")]
        public const string PersReader = "PersReader";
        [Description("PersWriter")]
        public const string PersWriter = "PersWriter";
        [Description("CounterReader")]
        public const string CounterReader = "CounterReader";
        [Description("CounterWriter")]
        public const string CounterWriter = "CounterWriter";
        [Description("Notifications")]
        public const string Notifications = "Notifications";
        [Description("CounterWriterNoLock")]
        public const string CounterWriterNoLock = "CounterWriterNoLock";
        [Description("SuperAdmin")]
        public const string SuperAdmin = "SuperAdmin";
        [Description("DownLoadReceipt")]
        public const string DownLoadReceipt = "DownLoadReceipt";
    }
}
