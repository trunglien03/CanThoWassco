using NienLuan2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NienLuan2.Controllers
{
    public class KhachHangController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
      
        public ActionResult ListKH(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            ViewBag.lt = new SelectList(db.LO_TRINH.OrderBy(x => x.LT_TEN), "LT_MA", "LT_TEN");
            ViewBag.httt = new SelectList(db.HINHTHUC_THANHTOAN.OrderBy(x => x.HTTT_TEN), "HTTT_MA", "HTTT_TEN");
            ViewBag.tt = new SelectList(db.TRANG_THAI.OrderBy(x => x.TT_TEN), "TT_MA", "TT_TEN");
            ViewBag.dt = new SelectList(db.DOI_TUONG.OrderBy(x => x.DT_TEN), "DT_MA", "DT_TEN");
            ViewBag.kv = new SelectList(db.KHU_VUC.OrderBy(x => x.KV_TEN), "KV_MA", "KV_TEN");
            ViewBag.qu = new SelectList(GetQuanList(), "KV_MA", "KV_TEN");

            IEnumerable<KHACH_HANG> model = db.KHACH_HANG;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.KH_HOTEN.Contains(searchString) || x.LO_TRINH.LT_MA.Contains(searchString) || x.HINHTHUC_THANHTOAN.HTTT_TEN.Contains(searchString)|| x.TRANG_THAI.TT_TEN.Contains(searchString) || x.DOI_TUONG.DT_MA.Contains(searchString) || x.KH_MA.ToString().Contains(searchString)).OrderByDescending(x => x.KH_MA);
            }

            ViewBag.SearchString = searchString;
            return View(model.OrderBy(x => x.TT_MA).ToPagedList(page, pageSize));
            //return View(model.ToList());
        }

 
        public ActionResult them_KH()
        {
            ViewBag.qu = new SelectList(GetQuanList(), "QUAN_MA", "QUAN_TEN");
            return View();
        }

        [HttpPost, ActionName("them_KH")]
        public ActionResult them_KH(KHACH_HANG Kh,FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.KHACH_HANG.Add(Kh);
                    db.SaveChanges();
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Error Save Data");
            }
            return RedirectToAction("ListKH");
        }

        public JsonResult EditKH(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var khachhang = db.KHACH_HANG.SingleOrDefault(m => m.KH_MA == id);

            return Json(khachhang, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_KH(int? id)
        {
            ViewBag.lt = new SelectList(db.LO_TRINH.OrderBy(x => x.LT_TEN), "LT_MA", "LT_TEN");
            ViewBag.httt = new SelectList(db.HINHTHUC_THANHTOAN.OrderBy(x => x.HTTT_TEN), "HTTT_MA", "HTTT_TEN");
            ViewBag.tt = new SelectList(db.TRANG_THAI.OrderBy(x => x.TT_TEN), "TT_MA", "TT_TEN");
            ViewBag.dt = new SelectList(db.DOI_TUONG.OrderBy(x => x.DT_TEN), "DT_MA", "DT_TEN");
            ViewBag.kv = new SelectList(db.KHU_VUC.OrderBy(x => x.KV_TEN), "KV_MA", "KV_TEN");

            //ViewBag.kv = new SelectList(GetKhuvucList(), "KV_MA", "KV_TEN");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACH_HANG khachhang = db.KHACH_HANG.SingleOrDefault(m => m.KH_MA == id);
            return View(khachhang);
        }

        [HttpPost, ActionName("Edit_KH1")]
        //[ValidateInput(false)]
        public ActionResult Edit_KH1(KHACH_HANG kh, FormCollection form, int? id)
        {


            
            kh.LT_MA = form["lt"].ToString();
            kh.HTTT_MA = form["httt"].ToString();
            kh.TT_MA = form["tt"].ToString();
            kh.DT_MA = form["dt"].ToString();
            kh.KV_MA = form["kv"].ToString();


            if (!ModelState.IsValid)
                return View(kh);
            if (TryUpdateModel(kh, "", new string[] { "KH_MA", "KH_HOTEN" }))
            {
                try
                {
                    db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("ListKH");
        }

        public ActionResult xoa_KH(int? id)
        {
            KHACH_HANG khachhang = db.KHACH_HANG.SingleOrDefault(s => s.KH_MA == id);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_KH1(int? id)
        {
            KHACH_HANG khachhang = db.KHACH_HANG.SingleOrDefault(s => s.KH_MA == id);
            db.KHACH_HANG.Remove(khachhang);
            db.SaveChanges();
            return RedirectToAction("ListKH");
        }



        public List<QUAN> GetQuanList()
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<QUAN> quan = db.QUANs.ToList();
            return quan;
        }
        public ActionResult GetPhuongList(string quan_ma)
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<PHUONG> selectList = db.PHUONGs.Where(x => x.QUAN_MA == quan_ma).ToList();
            ViewBag.phuong_list = new SelectList(selectList, "PHUONG_MA", "PHUONG_TEN");
            return PartialView("DisplayPHUONG");
        }
        public ActionResult GetKhuVucList(string phuong_ma)
        {
            Cty_CapNuocEntities db = new Cty_CapNuocEntities();
            List<KHU_VUC> selectList = db.KHU_VUC.Where(x => x.PHUONG_MA == phuong_ma).ToList();
            ViewBag.khuvuc_list = new SelectList(selectList, "KV_MA", "KV_TEN");
            return PartialView("DisplayQUAN");
        }
        [HttpGet]
        public ActionResult Cancel()
        {
            return RedirectToAction("ListKH");
        }


    }
}