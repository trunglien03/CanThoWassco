using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NienLuan2.Models
{
    public class Common
    {
        private static Cty_CapNuocEntities _Instance = new Cty_CapNuocEntities();

        public static Cty_CapNuocEntities Instance
        {
            get { return Common._Instance; }
            set { Common._Instance = value; }
        }

    }
}