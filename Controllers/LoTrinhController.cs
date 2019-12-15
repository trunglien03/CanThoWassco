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
    public class LoTrinhController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        // GET: NhanVien
        public ActionResult ListLT(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            ViewBag.vt = new SelectList(db.NHANVIENs.Where(x => (x.CV_MA == "CV03")), "NV_USERNAME", "NV_HOTEN");
            ViewBag.nv = new SelectList(db.NHANVIENs.Where(x => (x.CV_MA == "CV03")), "NV_USERNAME", "NV_HOTEN");
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<LO_TRINH> model = db.LO_TRINH;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.LT_TEN.Contains(searchString) || x.NHANVIEN.NV_HOTEN.Contains(searchString) || x.LT_MA.Contains(searchString)).OrderBy(x => x.LT_MA);
                
            }

            ViewBag.SearchString = searchString;
            return View(model.OrderBy(x => x.LT_MA).ToPagedList(page, pageSize));
            //return View(db.LO_TRINH);
        }
        public ActionResult AddLT()
        {
            ViewBag.vt = new SelectList(db.NHANVIENs.Where(x => (x.CV_MA == "CV03")), "NV_USERNAME", "NV_HOTEN");
            return View();
        }

        [HttpPost, ActionName("AddLT")]
        public ActionResult AddLT(LO_TRINH lt, FormCollection form)
        {
            if (db.LO_TRINH.Any(x => x.LT_MA == lt.LT_MA))
            {
                ViewBag.error = "Mã lộ trình này đã tồn tại!!!";
                return View(lt);
            }

            lt.NV_USERNAME = form["vt"].ToString();

            if (!ModelState.IsValid)
                return View(lt);
            db.LO_TRINH.Add(lt);
            db.SaveChanges();
            return RedirectToAction("ListLT");
        }


        public JsonResult EditLT(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var lotrinh = db.LO_TRINH.Find(id);

            return Json(lotrinh, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_LT(string id)
        {
            ViewBag.nv = new SelectList(db.NHANVIENs.Where(x => (x.CV_MA == "CV03")), "NV_USERNAME", "NV_HOTEN");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LO_TRINH lotrinh = db.LO_TRINH.Find(id);
            return View(lotrinh);
        }

        [HttpPost, ActionName("Edit_LT1")]
        //[ValidateInput(false)]
        ///[ValidateAntiForgeryToken]
        public ActionResult Edit_LT1(LO_TRINH lt, FormCollection form, string id)
        {
            lt.NV_USERNAME = form["nv"].ToString(); 
            if (!ModelState.IsValid)
                return View(lt);
            if (TryUpdateModel(lt, "", new string[] { "LT_MA", "LT_TEN" }))
            {
                try
                {
                    db.Entry(lt).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("ListLT");
        }

        public ActionResult Delete_LT(string id)
        {
            LO_TRINH lotrinh = db.LO_TRINH.SingleOrDefault(s => s.LT_MA == id);
            if (lotrinh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lotrinh);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_LT1(string id)
        {
            LO_TRINH lotrinh = db.LO_TRINH.SingleOrDefault(s => s.LT_MA == id);
            db.LO_TRINH.Remove(lotrinh);
            db.SaveChanges();
            return RedirectToAction("ListLT");
        }
    }
}