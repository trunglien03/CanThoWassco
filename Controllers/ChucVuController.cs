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
    public class ChucVuController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        public ActionResult ListCV(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<CHUCVU> model = db.CHUCVUs;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CV_TEN.Contains(searchString) || x.CV_MA.Contains(searchString)).OrderBy(x => x.CV_MA);
            }

            ViewBag.SearchString = searchString;
            return View(model.OrderBy(x => x.CV_MA).ToPagedList(page, pageSize));
            //return View(db.LO_TRINH);
        }
        public ActionResult AddCV()
        {
            return View();
        }

        [HttpPost, ActionName("AddCV")]
        public ActionResult AddCV(CHUCVU cv, FormCollection form)
        {
            if (db.CHUCVUs.Any(x => x.CV_MA == cv.CV_MA))
            {
                return RedirectToAction("ListCV", new { error = 1 });
            }
            if (!ModelState.IsValid)
                return View(cv);
            db.CHUCVUs.Add(cv);
            db.SaveChanges();
            return RedirectToAction("ListCV");
        }


        public JsonResult EditCV(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var chucvu = db.CHUCVUs.Find(id);

            return Json(chucvu, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_CV(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHUCVU chucvu = db.CHUCVUs.Find(id);
            return View(chucvu);
        }

        [HttpPost, ActionName("Edit_CV1")]
        //[ValidateInput(false)]
        ///[ValidateAntiForgeryToken]
        public ActionResult Edit_CV1(CHUCVU cv, FormCollection form, string id)
        {
            if (!ModelState.IsValid)
                return View(cv);
            if (TryUpdateModel(cv, "", new string[] { "CV_MA", "CV_TEN" }))
            {
                try
                {
                    db.Entry(cv).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("ListCV");
        }

        public ActionResult Delete_CV(string id)
        {
            CHUCVU chucvu = db.CHUCVUs.SingleOrDefault(s => s.CV_MA == id);
            if (chucvu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chucvu);
        }
        public ActionResult xoa_CV1(string id)
        {
            CHUCVU chucvu = db.CHUCVUs.SingleOrDefault(s => s.CV_MA == id);
            if (db.NHANVIENs.Any(x => x.CV_MA == id))
            {
                int error = 1;
                return RedirectToAction("ListCV", new { error });
            }
            db.CHUCVUs.Remove(chucvu);
            db.SaveChanges();
            return RedirectToAction("ListCV");
        }
    }
}