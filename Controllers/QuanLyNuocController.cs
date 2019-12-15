using NienLuan2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using Rotativa;
using System.Web.Mvc;

namespace NienLuan2.Controllers
{
    public class QuanLyNuocController : Controller
    {
        // GET: QuanLyNuoc
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        //Kỳ Ghi
        public ActionResult KyGhi(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            ViewBag.kg = new SelectList(db.KY_GHI);
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<KY_GHI> model = db.KY_GHI;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.KYGHI_TEN.Contains(searchString) || x.KYGHI_MA.Contains(searchString)).OrderBy(x => x.KYGHI_MA);

            }

            ViewBag.SearchString = searchString;
            return View(model.OrderBy(x => x.KYGHI_MA).ToPagedList(page, pageSize));
        }
        //Thêm Kỳ Ghi
        public ActionResult AddKG()
        {
            ViewBag.kg = new SelectList(GetKGList(), "KYGHI_MA", "KYGHI_TEN");
            return View();
        }


        public List<KY_GHI> GetKGList()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<KY_GHI> kyghi = db.KY_GHI.ToList();
            return kyghi;
        }

        public List<KHACH_HANG> GetKGList1()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<KHACH_HANG> kh = db.KHACH_HANG.ToList();
            return kh;
        }

        [HttpPost, ActionName("AddKG")]
        [AllowAnonymous]
        public ActionResult AddKG(KY_GHI kg, FormCollection form)
        {
            if (db.KY_GHI.Any(x => x.KYGHI_MA == kg.KYGHI_MA))
            {
                return RedirectToAction("KyGhi", new { error = 1 });
            }

            if (!ModelState.IsValid)

                return View(kg);

            db.KY_GHI.Add(kg);

            db.SaveChanges();
            //Lưu session
            Session["MaKG"] = kg.KYGHI_MA;

            string ma = Session["MaKG"].ToString();
            HttpCookie maKG = new HttpCookie("MaKG");
            //Gán cookie maKG = session ma
            maKG["MaKG"] = ma;
            //
            maKG.Expires = DateTime.Now.AddDays(31);
            Response.Cookies.Add(maKG);
            //Sửa ngày 07/07 xử lý lưu csn mới theo kỳ ghi mới và nếu khách hàng mới tạo thì kỳ đầu chỉ số cũ, tiêu thụ 1, tiêu thụ 2 = 0.

            var kh_mo = from a in db.KHACH_HANG where a.TT_MA == "TT01" select a;


            foreach (var item in kh_mo)
            {
                CHI_SO_NUOC csn = new CHI_SO_NUOC();


                var csn_truoc = db.CHI_SO_NUOC.Where(x => x.KYGHI_MA != ma && x.KH_MA == item.KH_MA).OrderByDescending(x => x.KYGHI_MA).FirstOrDefault();
                if (csn_truoc != null)
                {
                    csn.KH_MA = item.KH_MA;
                    csn.KYGHI_MA = ma;

                    csn.CSN_CU = csn_truoc.CSN_MOI;
                    csn.CSN_TIEUTHU1 = csn_truoc.CSN_TIEUTHU;
                    csn.CSN_TIEUTHU2 = csn_truoc.CSN_TIEUTHU1;
                    csn.TTPT_MA = "TTPT01";
                }
                if (csn_truoc == null)
                {
                    csn.KH_MA = item.KH_MA;
                    csn.KYGHI_MA = ma;

                    csn.CSN_CU = 0;
                    csn.CSN_TIEUTHU1 = 0;
                    csn.CSN_TIEUTHU2 = 0;
                    csn.TTPT_MA = "TTPT01";
                }
                if (ModelState.IsValid)
                {
                    db.CHI_SO_NUOC.Add(csn);

                }

            }
            db.SaveChanges();
            return RedirectToAction("ChiSoNuoc");
        }


        //Chỉ số nước

        public ActionResult ChiSoNuoc()
        {
            ViewBag.kyghi = new SelectList(GetKyghiList(), "KYGHI_MA", "KYGHI_TEN");
            return View();
        }
        public List<KY_GHI> GetKyghiList()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<KY_GHI> kyghi = db.KY_GHI.OrderByDescending(x => x.KYGHI_MA).ToList();
            return kyghi;
        }
        [HttpGet]
        public JsonResult GetChiSoNuoc(string id)
        {
            using (Cty_CapNuocEntities db = new Cty_CapNuocEntities())
            {
                var a = (from csn in db.CHI_SO_NUOC
                         join kh in db.KHACH_HANG on csn.KH_MA equals kh.KH_MA
                         join kyghi in db.KY_GHI on csn.KYGHI_MA equals kyghi.KYGHI_MA
                         join trangthai in db.TRANG_THAI_PT on csn.TTPT_MA equals trangthai.TTPT_MA
                         where csn.KYGHI_MA == id
                         select new ChiSoNuocModels
                         {
                             KH_MA = kh.KH_MA,
                             KH_HOTEN = kh.KH_HOTEN,
                             KYGHI_MA = kyghi.KYGHI_MA,
                             CSN_MA = csn.CSN_MA,
                             CSN_CU = csn.CSN_CU,
                             CSN_MOI = csn.CSN_MOI,
                             CSN_TIEUTHU = csn.CSN_TIEUTHU,
                             CSN_TIEUTHU1 = csn.CSN_TIEUTHU1,
                             CSN_TIEUTHU2 = csn.CSN_TIEUTHU2,
                             THANH_TIEN = csn.THANH_TIEN,
                             TTPT_TEN = trangthai.TTPT_TEN
                         }).ToList();
                return Json(a, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("BangGhi");
            return report;
        }

        public ActionResult PrintPartialViewToPdf(int id)
        {
            using (Cty_CapNuocEntities db = new Cty_CapNuocEntities())
            {
                var gia = db.CHI_SO_NUOC.FirstOrDefault(x => x.CSN_MA == id);
                string bangchu = ConvertDecimalToString(Convert.ToDecimal(gia.THANH_TIEN));
                var chisonuoc = (from csn1 in db.CHI_SO_NUOC
                                 join kh in db.KHACH_HANG on csn1.KH_MA equals kh.KH_MA
                                 join httt1 in db.HINHTHUC_THANHTOAN on kh.HTTT_MA equals httt1.HTTT_MA
                                 join dt1 in db.DOI_TUONG on kh.DT_MA equals dt1.DT_MA
                                 join dg1 in db.DONGIAs on dt1.DG_MA equals dg1.DG_MA
                                 join tt1 in db.TRANG_THAI_PT on csn1.TTPT_MA equals tt1.TTPT_MA
                                 select new ChiSoNuoc_KH_Models
                                 {
                                     KH_MA = csn1.KH_MA,
                                     KH_HOTEN = kh.KH_HOTEN,
                                     KH_DIACHILAPDAT = kh.KH_DIACHILAPDAT,
                                     HTTT_MA = httt1.HTTT_MA,
                                     HTTT_TEN = httt1.HTTT_TEN,
                                     KYGHI_MA = csn1.KYGHI_MA,
                                     CSN_MA = csn1.CSN_MA,
                                     CSN_MOI = csn1.CSN_MOI,
                                     CSN_CU = csn1.CSN_CU,
                                     CSN_TIEUTHU = csn1.CSN_TIEUTHU,
                                     CSN_TIEUTHU1 = csn1.CSN_TIEUTHU1,
                                     CSN_TIEUTHU2 = csn1.CSN_TIEUTHU2,
                                     DT_MA = kh.DT_MA,
                                     THANH_TIEN = csn1.THANH_TIEN,
                                     GIATIEN = dg1.GIATIEN,
                                     TTPT_MA = csn1.TTPT_MA,
                                     BANG_CHU = bangchu
                                 }).SingleOrDefault(m => m.CSN_MA == id);
                var report = new PartialViewAsPdf("~/Views/QuanLyNuoc/PrintCSN.cshtml", chisonuoc);
                return report;
            }

        }




        public ActionResult View_map(int id)
        {
            using (Cty_CapNuocEntities db = new Cty_CapNuocEntities())
            {
                var b = (from csn1 in db.CHI_SO_NUOC
                                 join kh in db.KHACH_HANG on csn1.KH_MA equals kh.KH_MA
                                 join httt1 in db.HINHTHUC_THANHTOAN on kh.HTTT_MA equals httt1.HTTT_MA
                                 join dt1 in db.DOI_TUONG on kh.DT_MA equals dt1.DT_MA
                                 join dg1 in db.DONGIAs on dt1.DG_MA equals dg1.DG_MA
                                 join tt1 in db.TRANG_THAI_PT on csn1.TTPT_MA equals tt1.TTPT_MA
                                 select new ChiSoNuoc_KH_Models
                                 {
                                     KH_MA = csn1.KH_MA,
                                     KH_HOTEN = kh.KH_HOTEN,
                                     KH_DIACHILAPDAT = kh.KH_DIACHILAPDAT,
                                     HTTT_MA = httt1.HTTT_MA,
                                     HTTT_TEN = httt1.HTTT_TEN,
                                     KYGHI_MA = csn1.KYGHI_MA,
                                     CSN_MA = csn1.CSN_MA,
                                     CSN_MOI = csn1.CSN_MOI,
                                     CSN_CU = csn1.CSN_CU,
                                     CSN_TIEUTHU = csn1.CSN_TIEUTHU,
                                     CSN_TIEUTHU1 = csn1.CSN_TIEUTHU1,
                                     CSN_TIEUTHU2 = csn1.CSN_TIEUTHU2,
                                     DT_MA = kh.DT_MA,
                                     THANH_TIEN = csn1.THANH_TIEN,
                                     GIATIEN = dg1.GIATIEN,
                                     TTPT_MA = csn1.TTPT_MA,
                                 }).SingleOrDefault(m => m.CSN_MA == id);
                return View(b);
            }

        }



        //Bảng ghi nước

        //chi so nuoc-------------------------------
        public ActionResult BangGhi(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            ViewBag.vt = new SelectList(db.CHI_SO_NUOC);
            if (error == 1)
                ViewBag.Loi = 1;
            ViewBag.tt = new SelectList(db.TRANG_THAI_PT.OrderBy(x => x.TTPT_MA), "TTPT_MA", "TTPT_TEN");
            string username = (Session["TaiKhoan"] as NHANVIEN).NV_USERNAME;
            var a = (from csn1 in db.CHI_SO_NUOC
                     join kh in db.KHACH_HANG on csn1.KH_MA equals kh.KH_MA
                     join lt1 in db.LO_TRINH on kh.LT_MA equals lt1.LT_MA
                     join nv1 in db.NHANVIENs on lt1.NV_USERNAME equals nv1.NV_USERNAME
                     join dt1 in db.DOI_TUONG on kh.DT_MA equals dt1.DT_MA
                     join dg1 in db.DONGIAs on dt1.DG_MA equals dg1.DG_MA
                     join tt1 in db.TRANG_THAI_PT on csn1.TTPT_MA equals tt1.TTPT_MA
                     where nv1.NV_USERNAME == username
                     select new ChiSoNuoc_KH_Models
                     {
                         KH_MA = csn1.KH_MA,
                         KH_HOTEN = kh.KH_HOTEN,
                         KH_DIACHILAPDAT = kh.KH_DIACHILAPDAT,
                         LT_MA = lt1.LT_MA,
                         NV_USERNAME = nv1.NV_USERNAME,
                         NV_HOTEN = nv1.NV_HOTEN,
                         KYGHI_MA = csn1.KYGHI_MA,
                         CSN_MA = csn1.CSN_MA,
                         CSN_MOI = csn1.CSN_MOI,
                         CSN_CU = csn1.CSN_CU,
                         CSN_TIEUTHU = csn1.CSN_TIEUTHU,
                         CSN_TIEUTHU1 = csn1.CSN_TIEUTHU1,
                         CSN_TIEUTHU2 = csn1.CSN_TIEUTHU2,
                         DT_MA = kh.DT_MA,
                         THANH_TIEN = csn1.THANH_TIEN,
                         GIATIEN = dg1.GIATIEN,
                         TTPT_MA = csn1.TTPT_MA,
                         TTPT_TEN = tt1.TTPT_TEN
                     }).ToList().OrderBy(x =>x.TTPT_MA);
            return View(a);
        }

        public JsonResult EditCSN(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            // var csn = db.CHI_SO_NUOC.SingleOrDefault(m => m.CSN_MA == id);
            var chisonuoc = (from csn1 in db.CHI_SO_NUOC
                     join kh in db.KHACH_HANG on csn1.KH_MA equals kh.KH_MA
                     join dt1 in db.DOI_TUONG on kh.DT_MA equals dt1.DT_MA
                     join dg1 in db.DONGIAs on dt1.DG_MA equals dg1.DG_MA
                     join tt1 in db.TRANG_THAI_PT on csn1.TTPT_MA equals tt1.TTPT_MA
                     select new ChiSoNuoc_KH_Models
                     {
                         KH_MA = csn1.KH_MA,
                         KH_HOTEN = kh.KH_HOTEN,
                         KH_DIACHILAPDAT = kh.KH_DIACHILAPDAT,
                         KYGHI_MA = csn1.KYGHI_MA,
                         CSN_MA = csn1.CSN_MA,
                         CSN_MOI = csn1.CSN_MOI,
                         CSN_CU = csn1.CSN_CU,
                         CSN_TIEUTHU = csn1.CSN_TIEUTHU,
                         CSN_TIEUTHU1 = csn1.CSN_TIEUTHU1,
                         CSN_TIEUTHU2 = csn1.CSN_TIEUTHU2,
                         DT_MA = kh.DT_MA,
                         THANH_TIEN = csn1.THANH_TIEN,
                         GIATIEN = dg1.GIATIEN,
                         TTPT_MA = csn1.TTPT_MA
                     }).SingleOrDefault(m => m.CSN_MA == id);
            return Json(chisonuoc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_CSN(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHI_SO_NUOC csn = db.CHI_SO_NUOC.SingleOrDefault(m => m.CSN_MA == id);
            return View(csn);
        }

        [AllowAnonymous,HttpPost, ActionName("Edit_CSN1")]
        //[ValidateInput(false)]
        public ActionResult Edit_CSN1(CHI_SO_NUOC csn, FormCollection form, int? id)
        {
            if (!ModelState.IsValid)
                return View(csn);
            if (TryUpdateModel(csn, "", new string[] { "KH_MA", "KYGHI_MA", "CSN_MA", "CSN_MOI", "CSN_CU", "CSN_TIEUTHU", "CSN_TIEUTHU1", "CSN_TIEUTHU2","TTPT_MA","THANH_TIEN" }))
            {
                try
                {
                    db.Entry(csn).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("BangGhi");
        }


        public static string ConvertDecimalToString(decimal number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            return str + "đồng chẵn";
        }


    }
}









