using NienLuan2.Models;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NienLuan2.Controllers
{
    public class PhieuThuController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        // GET: PhieuThu
        public ActionResult PTCSN_NAM(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            List<KHACH_HANG> khachhang = db.KHACH_HANG.ToList();
            List<NHANVIEN> nhanvien = db.NHANVIENs.ToList();
            List<KY_GHI> kyghi = db.KY_GHI.ToList();
            List<LO_TRINH> lotrinh = db.LO_TRINH.ToList();
            var model = (from csn in db.CHI_SO_NUOC
                         join kh in db.KHACH_HANG on csn.KH_MA equals kh.KH_MA
                         join kg in db.KY_GHI on csn.KYGHI_MA equals kg.KYGHI_MA
                         join lt in db.LO_TRINH on kh.LT_MA equals lt.LT_MA
                         join nv in db.NHANVIENs on lt.NV_USERNAME equals nv.NV_USERNAME
                         orderby kg.KYGHI_TEN descending
                         select new ChiSoNuocModels
                         {
                             KH_MA = kh.KH_MA,
                             KYGHI_MA = kg.KYGHI_MA,
                             CSN_MA = csn.CSN_MA,
                             CSN_CU = csn.CSN_CU,
                             CSN_MOI = csn.CSN_MOI,
                             CSN_TIEUTHU = csn.CSN_TIEUTHU,
                             CSN_TIEUTHU1 = csn.CSN_TIEUTHU1,
                             CSN_TIEUTHU2 = csn.CSN_TIEUTHU2,
                             NV_HOTEN = nv.NV_HOTEN,
                             NV_USERNAME = nv.NV_USERNAME,
                             LT_MA = lt.LT_MA,
                             TT_MA = kh.TT_MA,
                             THANH_TIEN = csn.THANH_TIEN
                         }).ToList().OrderByDescending(x=>x.KYGHI_MA);
            return View(model);
        }
        public ActionResult PTCSN()
        {
            ViewBag.kyghi = new SelectList(GetKyghiList(), "KYGHI_MA", "KYGHI_TEN");
            ViewBag.Ltrinh = new SelectList(GetLoTrinhList(), "LT_MA", "LT_TEN");
            return View();
        }
        public List<KY_GHI> GetKyghiList()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<KY_GHI> kyghi = db.KY_GHI.OrderByDescending(x => x.KYGHI_MA).ToList();
            return kyghi;
        }
        public List<LO_TRINH> GetLoTrinhList()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<LO_TRINH> Lotrinh = db.LO_TRINH.OrderBy(x => x.LT_MA).ToList();
            return Lotrinh;
        }

        public List<NHANVIEN> GetNhanVienList()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<NHANVIEN> nhanvien = db.NHANVIENs.Where(x => (x.CV_MA == "CV03")).ToList();
            return nhanvien;
        }
        [HttpGet]
        // GET: PhieuThu
        public JsonResult GetPTCSN(string id, string ltrinh)
        {
            using (Cty_CapNuocEntities db = new Cty_CapNuocEntities())
            {
                string tongtien = db.CHI_SO_NUOC.Where(x => x.KYGHI_MA == id && x.KHACH_HANG.LT_MA == ltrinh).Sum(x => x.CSN_TIEUTHU).ToString();
                var a = (from csn in db.CHI_SO_NUOC
                         join kh in db.KHACH_HANG on csn.KH_MA equals kh.KH_MA
                         join kg in db.KY_GHI on csn.KYGHI_MA equals kg.KYGHI_MA
                         join lt in db.LO_TRINH on kh.LT_MA equals lt.LT_MA
                         join nv in db.NHANVIENs on lt.NV_USERNAME equals nv.NV_USERNAME
                         where csn.KYGHI_MA == id && lt.LT_MA == ltrinh
                         select new ChiSoNuocModels
                         {
                             KH_MA = kh.KH_MA,
                             KYGHI_MA = kg.KYGHI_MA,
                             CSN_MA = csn.CSN_MA,
                             CSN_TIEUTHU = csn.CSN_TIEUTHU,
                             NV_HOTEN = nv.NV_HOTEN,
                             NV_USERNAME = nv.NV_USERNAME,
                             LT_MA = lt.LT_MA,
                             TT_MA = kh.TT_MA,
                             TONG_TIEN = tongtien
                         }).ToList();
                return Json(a, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetDoanhThu(string id, string nhanvien)
        {
            using (Cty_CapNuocEntities db = new Cty_CapNuocEntities())
            {
                string tongtien = db.CHI_SO_NUOC.Where(x => x.KYGHI_MA == id && x.KHACH_HANG.LO_TRINH.NV_USERNAME == nhanvien).Sum(x => x.THANH_TIEN).ToString();
                var b = (from csn in db.CHI_SO_NUOC
                         join kh in db.KHACH_HANG on csn.KH_MA equals kh.KH_MA
                         join kg in db.KY_GHI on csn.KYGHI_MA equals kg.KYGHI_MA
                         join lt in db.LO_TRINH on kh.LT_MA equals lt.LT_MA
                         join nv in db.NHANVIENs on lt.NV_USERNAME equals nv.NV_USERNAME
                         join dt in db.DOI_TUONG on kh.DT_MA equals dt.DT_MA
                         join dg in db.DONGIAs on dt.DG_MA equals dg.DG_MA
                         where csn.KYGHI_MA == id && nv.NV_USERNAME == nhanvien
                         select new DoanhThuModels
                         {
                             KH_MA = kh.KH_MA,
                             KYGHI_MA = kg.KYGHI_MA,
                             CSN_MA = csn.CSN_MA,
                             CSN_TIEUTHU = csn.CSN_TIEUTHU,
                             NV_HOTEN = nv.NV_HOTEN,
                             NV_USERNAME = nv.NV_USERNAME,
                             LT_MA = lt.LT_MA,
                             DT_MA = dt.DT_MA,
                             DT_TEN = dt.DT_TEN,
                             DG_MA = dg.DG_MA,
                             GIATIEN = dg.GIATIEN,
                             TT_MA = kh.TT_MA,
                             TONG_TIEN = tongtien,
                             THANHTIEN = csn.THANH_TIEN
                         }).ToList().OrderBy(x=> x.LT_MA);
                return Json(b, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DoanhThu()
        {
            
            ViewBag.kyghi = new SelectList(GetKyghiList(), "KYGHI_MA", "KYGHI_TEN");
            ViewBag.nhanvien = new SelectList(GetNhanVienList(), "NV_USERNAME", "NV_HOTEN");
            DoanhThuModels dt = new DoanhThuModels();
            return View();
        }

    }
}