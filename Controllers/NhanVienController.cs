using NienLuan2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;



namespace NienLuan2.Controllers
{
    public class NhanVienController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();

        public ActionResult ListNV(string searchString,int? error, int page = 1, int pageSize = 10)
        {
            ViewBag.vt = new SelectList(db.VAITROes, "VT_MA", "VT_TEN");
            ViewBag.cv = new SelectList(db.CHUCVUs, "CV_MA", "CV_TEN");
            ViewBag.pb = new SelectList(db.PHONGBANs, "PB_MA", "PB_TEN");
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<NHANVIEN> model = db.NHANVIENs;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.NV_HOTEN.Contains(searchString) || x.CHUCVU.CV_TEN.Contains(searchString) || x.VAITRO.VT_MA.Contains(searchString) || x.PHONGBAN.PB_TEN.Contains(searchString) ||  x.NV_USERNAME.Contains(searchString)).OrderByDescending(x => x.NV_HOTEN);
            }

            ViewBag.SearchString = searchString;
            return View(model.OrderBy(x => x.PB_MA).ToPagedList(page, pageSize));
            //  return View(db.NHANVIENs);
        }

        public ActionResult AddNV()
        {
            ViewBag.vt = new SelectList(db.VAITROes.OrderBy(x => x.VT_MA), "VT_MA", "VT_TEN");
            ViewBag.cv = new SelectList(db.CHUCVUs.OrderBy(x => x.CV_TEN), "CV_MA", "CV_TEN");
            ViewBag.pb = new SelectList(db.PHONGBANs.OrderBy(x => x.PB_TEN), "PB_MA", "PB_TEN");
            return View();
        }

        [HttpPost, ActionName("AddNV")]
        public ActionResult AddNV(NHANVIEN Nv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.NHANVIENs.Add(Nv);
                    db.SaveChanges();
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Error Save Data");
            }
            return RedirectToAction("ListNV");
        }


        public JsonResult EditNV(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nhanvien = db.NHANVIENs.Find(id);

            return Json(nhanvien, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_NV(string id)
        {               
            ViewBag.vt = new SelectList(db.VAITROes, "VT_MA", "VT_TEN");
            ViewBag.cv = new SelectList(db.CHUCVUs, "CV_MA", "CV_TEN");
            ViewBag.pb = new SelectList(db.PHONGBANs, "PB_MA", "PB_TEN");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanvien = db.NHANVIENs.Find(id);
            return View(nhanvien);
        }

        [HttpPost, ActionName("Edit_NV1")]
        //[ValidateInput(false)]
        ///[ValidateAntiForgeryToken]
        public ActionResult Edit_NV1(NHANVIEN nv, FormCollection form, string id)
        {

            ViewBag.vt = new SelectList(db.VAITROes, "VT_MA", "VT_TEN");
            ViewBag.cv = new SelectList(db.CHUCVUs, "CV_MA", "CV_TEN",nv.VT_MA);
            ViewBag.pb = new SelectList(db.PHONGBANs, "PB_MA", "PB_TEN",nv.PB_MA);



            nv.VT_MA = form["vt"].ToString();
            nv.CV_MA = form["cv"].ToString();
            nv.PB_MA = form["pb"].ToString();
            if (!ModelState.IsValid)
                return View(nv);
            if (TryUpdateModel(nv, "", new string[] { "NV_USERNAME", "NV_HOTEN" }))
            {
                try
                {
                    db.Entry(nv).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("ListNV");
        }

        public ActionResult Delete_NV(string id)
        {
            NHANVIEN nhanvien = db.NHANVIENs.SingleOrDefault(s => s.NV_USERNAME == id);
            if (nhanvien == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nhanvien);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_NV1(string id)
        {
            NHANVIEN nhanvien = db.NHANVIENs.SingleOrDefault(s => s.NV_USERNAME == id);
            if (db.LO_TRINH.Any(x => x.NV_USERNAME == id))
            {
                //ViewBag.error = "Vui lòng xóa nhân viên thuộc phòng ban trước !!";
                int error = 1;
                return RedirectToAction("ListNV", new { error });
            }
            db.NHANVIENs.Remove(nhanvien);
            db.SaveChanges();
            return RedirectToAction("ListNV");
        }
        public JsonResult detail_NV(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nv = db.NHANVIENs.Find(id);
            return Json(nv, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Cancel()
        {
            return RedirectToAction("ListNV");
        }
    }
}