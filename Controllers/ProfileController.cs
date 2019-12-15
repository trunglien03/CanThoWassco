using NienLuan2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NienLuan2.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();

        [HttpGet]
        public ActionResult Index1()
        {
            NHANVIEN nv = (NHANVIEN)Session["TaiKhoan"];

            return View(nv);
        }

        [HttpPost]
        public ActionResult ChangeAvatar(HttpPostedFileBase file)
        {
            NHANVIEN nv = (NHANVIEN)Session["TaiKhoan"];
            //string[] filename = file.FileName.Split('.');
            string newPath = Path.Combine(Server.MapPath("~/Content/Image/" + nv.NV_USERNAME + ".png"));
            file.SaveAs(newPath);
            NHANVIEN nv1 = db.NHANVIENs.FirstOrDefault(avt => avt.NV_USERNAME == nv.NV_USERNAME);
            nv1.NV_AVATAR = nv.NV_USERNAME + ".png";
            db.SaveChanges();
            return RedirectToAction("Index1","Profile");
        }

        public JsonResult ChangePassword(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var nhanvien = db.NHANVIENs.Find(id);

            return Json(nhanvien, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Change_Password(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanvien = db.NHANVIENs.Find(id);
            return View(nhanvien);
        }

        [HttpPost, ActionName("Change_Password1")]
        public ActionResult Change_Password1(string id, string password)
        {
            NHANVIEN nv = new NHANVIEN();
            if (!ModelState.IsValid)
                return View(nv);

            if (TryUpdateModel(nv, "", new string[] { "NV_USERNAME", "NV_PASSWORD" }))
            {
                try
                {
                    Cty_CapNuocEntities cty_CapNuocEntities = new Cty_CapNuocEntities();
                    cty_CapNuocEntities.NHANVIENs.Attach(nv);
                    cty_CapNuocEntities.Entry(nv).Property(x => x.NV_PASSWORD).IsModified = true;
                    cty_CapNuocEntities.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("Index1", "Profile");
        }      
    }
}