using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NienLuan2.Models
{
    public class ChiSoNuoc_KH_Models
    {
        public int KH_MA { get; set; }

        public string KH_HOTEN { get; set; }
        public string KH_DIACHILAPDAT { get; set; }
        public string HTTT_MA { get; set; }
        public string HTTT_TEN { get; set; }
        public string LT_MA { get; set; }
        public string NV_USERNAME { get; set; }
        public string NV_HOTEN { get; set; }
        public string KYGHI_MA { get; set; }
        public int CSN_MA { get; set; }
        public string DT_MA { get; set; }
        public string TTPT_MA { get; set; }
        public string TTPT_TEN { get; set; }
        public string BANG_CHU { get; set; }
        public Nullable<double> CSN_MOI { get; set; }
        public Nullable<double> CSN_CU { get; set; }
        public Nullable<double> CSN_TIEUTHU { get; set; }
        public Nullable<double> CSN_TIEUTHU1 { get; set; }
        public Nullable<double> CSN_TIEUTHU2 { get; set; }
        public Nullable<double> GIATIEN { get; set; }
        public Nullable<double> THANH_TIEN { get; set; }
    }
}