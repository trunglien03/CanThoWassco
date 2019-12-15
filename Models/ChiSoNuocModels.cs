using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NienLuan2.Models
{
    public class ChiSoNuocModels
    {
        public int KH_MA { get; set; }
        public string KH_HOTEN { get; set; }
        public string KYGHI_MA { get; set; }
        public string KYGHI_TEN { get; set; }
        public int CSN_MA { get; set; }
        public string LT_MA { get; set; }
        public string NV_USERNAME { get; set; }
        public string NV_HOTEN { get; set; }
        public string TT_MA { get; set; }
        public string TONG_TIEN { get; set; }
        public string TTPT_MA { get; set; }
        public string TTPT_TEN { get; set; }
        public Nullable<double> CSN_MOI { get; set; }
        public Nullable<double> CSN_CU { get; set; }
        public Nullable<double> CSN_TIEUTHU { get; set; }
        public Nullable<double> CSN_TIEUTHU1 { get; set; }
        public Nullable<double> CSN_TIEUTHU2 { get; set; }
        public Nullable<double> THANH_TIEN { get; set; }
    }
}