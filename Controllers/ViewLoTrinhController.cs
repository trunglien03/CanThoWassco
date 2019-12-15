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
    public class ViewLoTrinhController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        public ActionResult ListLT()
        {
            string username = (Session["TaiKhoan"] as NHANVIEN).NV_USERNAME;
            var rp = from i in db.LO_TRINH
                     where i.NV_USERNAME == username
                     orderby i.LT_MA
                     select i;
            return View(rp.ToList());
        }
    }
}