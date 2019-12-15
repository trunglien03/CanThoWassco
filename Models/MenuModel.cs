using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NienLuan2.Models
{
    public class MenuModel
    {
        public string MAIN_NAME { get; set; }
        public int MAIN_ID { get; set; }
        public string MAIN_ICON{ get; set; }
        public string SUB_NAME { get; set; }
        public int SUB_ID { get; set; }
        public string SUB_CONTROLLER { get; set; }
        public string SUB_ACTION { get; set; }
        public string VT_MA { get; set; }
        public string VT_TEN { get; set; }
    }
}