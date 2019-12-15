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
    public class MenuController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        public ActionResult ListMN(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            ViewBag.vt = new SelectList(db.VAITROes.OrderBy(x => x.VT_MA), "VT_MA", "VT_TEN");
            ViewBag.main = new SelectList(db.MAIN_MENU.OrderBy(x => x.MAIN_ID), "MAIN_ID", "MAIN_NAME");
            ViewBag.vt1 = new SelectList(db.VAITROes.OrderBy(x => x.VT_MA), "VT_MA", "VT_TEN");
            ViewBag.main1 = new SelectList(db.MAIN_MENU.OrderBy(x => x.MAIN_ID), "MAIN_ID", "MAIN_NAME");
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<SUB_MENU> model = db.SUB_MENU;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.SUB_CONTROLLER.Contains(searchString) || x.SUB_ACTION.Contains(searchString)|| x.MAIN_MENU.MAIN_NAME.Contains(searchString) || x.VAITRO.VT_TEN.Contains(searchString)).OrderBy(x => x.VT_MA);
            }

            ViewBag.SearchString = searchString;
            return View(model.ToList());
        }

        public ActionResult AddMN()
        {
            ViewBag.vt = new SelectList(db.VAITROes.OrderBy(x => x.VT_MA), "VT_MA", "VT_TEN");
            ViewBag.main = new SelectList(db.MAIN_MENU.OrderBy(x => x.MAIN_ID), "MAIN_ID", "MAIN_NAME");
            return View();
        }

        [HttpPost, ActionName("AddMN")]
        public ActionResult AddLT(SUB_MENU sub, FormCollection form)
        {
            if (db.SUB_MENU.Any(x => x.SUB_ID == sub.SUB_ID))
            {
                return View(sub);
            }

            sub.VT_MA = form["vt"].ToString();
            sub.MAIN_ID = int.Parse(form["main"].ToString());

            if (!ModelState.IsValid)
                return View(sub);
            db.SUB_MENU.Add(sub);
            db.SaveChanges();
            return RedirectToAction("ListMN");
        }

        public ActionResult Delete_MN(int? id)
        {
            SUB_MENU menu = db.SUB_MENU.SingleOrDefault(s => s.SUB_ID == id);
            if (menu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(menu);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_MN1(int? id)
        {
            SUB_MENU menu = db.SUB_MENU.SingleOrDefault(s => s.SUB_ID == id);
            db.SUB_MENU.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("ListMN");
        }

        public JsonResult EditMN(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var submenu = db.SUB_MENU.Find(id);

            return Json(submenu, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit_MN(int? id)
        {
            ViewBag.vt1 = new SelectList(db.VAITROes.OrderBy(x => x.VT_MA), "VT_MA", "VT_TEN");
            ViewBag.main1 = new SelectList(db.MAIN_MENU.OrderBy(x => x.MAIN_ID), "MAIN_ID", "MAIN_NAME");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUB_MENU submenu = db.SUB_MENU.Find(id);
            return View(submenu);
        }

        [HttpPost, ActionName("Edit_MN1")]
        public ActionResult Edit_MN1(SUB_MENU sub, FormCollection form, int? id)
        {
            sub.VT_MA = form["vt1"].ToString();
            sub.MAIN_ID = int.Parse(form["main1"].ToString());


            if (!ModelState.IsValid)
                return View(sub);
            if (TryUpdateModel(sub, "", new string[] { "SUB_ID", "SUB_NAME" }))
            {
                try
                {
                    db.Entry(sub).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi");
                }

            }
            return RedirectToAction("ListMN");
        }


        public ActionResult ListVT(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<VAITRO> model = db.VAITROes;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.VT_MA.Contains(searchString) || x.VT_TEN.Contains(searchString)).OrderBy(x => x.VT_MA);
            }

            ViewBag.SearchString = searchString;
            return View(model.OrderBy(x => x.VT_MA).ToPagedList(page, pageSize));
            //return View(db.LO_TRINH);
        }

        public ActionResult AddVT()
        {
            return View();
        }

        [HttpPost, ActionName("AddVT")]
        public ActionResult AddVT(VAITRO vt, FormCollection form)
        {
            if (db.VAITROes.Any(x => x.VT_MA == vt.VT_MA))
            {
                return View(vt);
            }

            if (!ModelState.IsValid)
                return View(vt);
            db.VAITROes.Add(vt);
            db.SaveChanges();
            return RedirectToAction("ListVT");
        }

        public ActionResult Delete_VT(string id)
        {
            VAITRO vaitro = db.VAITROes.SingleOrDefault(s => s.VT_MA == id);
            if (vaitro == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(vaitro);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_VT1(string id)
        {
            VAITRO vaitro = db.VAITROes.SingleOrDefault(s => s.VT_MA == id);
            if (db.NHANVIENs.Any(x => x.VT_MA == id))
            {
                //ViewBag.error = "Vui lòng xóa nhân viên thuộc phòng ban trước !!";
                int error = 1;
                return RedirectToAction("ListVT", new { error });
            }
            db.VAITROes.Remove(vaitro);
            db.SaveChanges();
            return RedirectToAction("ListVT");
        }



        public ActionResult ListMAIN(string searchString, int? error, int page = 1, int pageSize = 10)
        {
            if (error == 1)
                ViewBag.Loi = 1;

            IEnumerable<MAIN_MENU> model = db.MAIN_MENU;

            return View(model.OrderBy(x => x.MAIN_ID).ToPagedList(page, pageSize));
            //return View(db.LO_TRINH);
        }

        public ActionResult AddMAIN()
        {
            return View();
        }

        [HttpPost, ActionName("AddMAIN")]
        public ActionResult AddMAIN(MAIN_MENU main, FormCollection form)
        {
            if (db.MAIN_MENU.Any(x => x.MAIN_ID == main.MAIN_ID))
            {
                return View(main);
            }

            if (!ModelState.IsValid)
                return View(main);
            db.MAIN_MENU.Add(main);
            db.SaveChanges();
            return RedirectToAction("ListMAIN");
        }

        public ActionResult Delete_MAIN(int? id)
        {
            MAIN_MENU main = db.MAIN_MENU.SingleOrDefault(s => s.MAIN_ID == id);
            if (main == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(main);
        }
        //[HttpPost, ActionName("xoa_NV")]
        public ActionResult xoa_MAIN1(int? id)
        {
            MAIN_MENU main = db.MAIN_MENU.SingleOrDefault(s => s.MAIN_ID == id);
            if (db.SUB_MENU.Any(x => x.MAIN_ID == id))
            {
                //ViewBag.error = "Vui lòng xóa nhân viên thuộc phòng ban trước !!";
                int error = 1;
                return RedirectToAction("ListMAIN", new { error });
            }
            db.MAIN_MENU.Remove(main);
            db.SaveChanges();
            return RedirectToAction("ListMAIN");
        }

    }
}