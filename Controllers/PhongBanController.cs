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

    public class PhongBanController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        // GET: PhongBan
        public ActionResult ListPB(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<PHONGBAN> model = db.PHONGBANs;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.PB_TEN.Contains(searchString) || x.PB_MA.Contains(searchString)).OrderByDescending(x => x.PB_TEN);
            }

            ViewBag.SearchString = searchString;
            return View(model.OrderByDescending(x => x.PB_TEN).ToPagedList(page, pageSize));
        }

        public JsonResult EditPB(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var phongban = db.PHONGBANs.Find(id);

            return Json(phongban, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_PB(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHONGBAN phongban = db.PHONGBANs.Find(id);
            return View(phongban);
        }

        [HttpPost, ActionName("sua_PB1")]
        public ActionResult sua_PB1(PHONGBAN pb, FormCollection form, string id)
        {
            if (!ModelState.IsValid)
                return View(pb);
            if (TryUpdateModel(pb, "", new string[] { "PB_MA", "PB_TEN" }))
            {
                try
                {
                    db.Entry(pb).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("ListPB");
        }


        public ActionResult Delete_PB(string id)
        {
            PHONGBAN phongban = db.PHONGBANs.SingleOrDefault(s => s.PB_MA == id);
            if (phongban == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phongban);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_PB1(string id)
        {
            PHONGBAN phongban = db.PHONGBANs.SingleOrDefault(s => s.PB_MA == id);
            if (db.NHANVIENs.Any(x => x.PB_MA == id))
            {
                //ViewBag.error = "Vui lòng xóa nhân viên thuộc phòng ban trước !!";
                int error = 1;
                return RedirectToAction("ListPB", new { error });
            }
            db.PHONGBANs.Remove(phongban);
            db.SaveChanges();
            return RedirectToAction("ListPB");
        }

        public JsonResult detail_PB(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var pb = db.PHONGBANs.Find(id);
            return Json(pb, JsonRequestBehavior.AllowGet);
        }


        public ActionResult them_PB()
        {
            return View();
        }
        [HttpPost, ActionName("them_PB")]
        [ValidateInput(false)]
        public ActionResult them_PB(PHONGBAN pb)
        {
            if (db.PHONGBANs.Any(x => x.PB_MA == pb.PB_MA))
            {
                ViewBag.error = "Mã chức vụ này đã tồn tại!!!";
                return View(pb);
            }

            if (!ModelState.IsValid)
                return View(pb);
            db.PHONGBANs.Add(pb);
            db.SaveChanges();
            return RedirectToAction("ListPB");
        }

    }
}