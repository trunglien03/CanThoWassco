using NienLuan2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NienLuan2.Controllers
{
    public class DangNhapController : Controller
    {
        Cty_CapNuocEntities db = new Cty_CapNuocEntities();
        // GET: DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult DangNhap(string maNhanVien, string matKhau)
        //{
        //    var nhanvien = db.NHANVIENs.SingleOrDefault(s => s.NV_USERNAME == maNhanVien && s.NV_PASSWORD == matKhau);
        //    if (nhanvien != null)
        //    {
        //        Session["MaNV"] = nhanvien.NV_USERNAME;
        //        Session["TenNV"] = nhanvien.NV_HOTEN;
        //        Session["MatKhau"] = nhanvien.NV_PASSWORD;
        //        Session["VaiTro"] = nhanvien.VT_MA;
        //        //gán session Tài khoản
        //        Session["online"] = nhanvien;
        //        Session["TaiKhoan"] = nhanvien as NHANVIEN;
        //        if(nhanvien.VT_MA == "VT02")
        //        {
        //        return RedirectToAction("ListKH", "KhachHang");
        //        }
        //        return RedirectToAction("ListNV", "NhanVien");
        //    }
        //    ViewBag.message = "Sai tài khoản hoặc mật khẩu";
        //    return View();
        //}

        [HttpPost]
        public ActionResult DangNhap(string maNhanVien, string matKhau)
        {
            var nhanvien = db.NHANVIENs.SingleOrDefault(s => s.NV_USERNAME == maNhanVien && s.NV_PASSWORD == matKhau);
            using (Cty_CapNuocEntities _entity = new Cty_CapNuocEntities())  // out Entity name is "SampleMenuMasterDBEntites"  
                if (nhanvien != null)
            {
                Session["MaNV"] = nhanvien.NV_USERNAME;
                Session["TenNV"] = nhanvien.NV_HOTEN;
                Session["MatKhau"] = nhanvien.NV_PASSWORD;
                Session["VaiTro"] = nhanvien.VT_MA;
                //gán session Tài khoản
                //Session["online"] = nhanvien;
                Session["TaiKhoan"] = nhanvien as NHANVIEN;
                List<MenuModel> _menus = _entity.SUB_MENU.Where(x => x.VT_MA == nhanvien.VT_MA).Select(x => new MenuModel
                {
                    MAIN_ID = x.MAIN_MENU.MAIN_ID,
                    MAIN_ICON = x.MAIN_MENU.MAIN_ICON,
                    MAIN_NAME = x.MAIN_MENU.MAIN_NAME,
                    SUB_ID = x.SUB_ID,
                    SUB_NAME = x.SUB_NAME,
                    SUB_CONTROLLER = x.SUB_CONTROLLER,
                    SUB_ACTION = x.SUB_ACTION,
                    VT_MA = x.VT_MA,
                    VT_TEN = x.VAITRO.VT_TEN
                }).ToList(); //Get the Menu details from entity and bind it in MenuModels list.  
                FormsAuthentication.SetAuthCookie(nhanvien.NV_USERNAME, false); // set the formauthentication cookie  
                Session["LoginCredentials"] = nhanvien; // Bind the _logincredentials details to "LoginCredentials" session  
                Session["MenuMaster"] = _menus; //Bind the _menus list to MenuMaster session  
                Session["UserName"] = nhanvien.NV_USERNAME;
                    if (nhanvien.VT_MA == "VT02")
                {
                return RedirectToAction("ListKH", "KhachHang");
                } else if (nhanvien.VT_MA == "VT01")
                    {
                return RedirectToAction("ListNV", "NhanVien");
                    }
                return RedirectToAction("ListLT", "ViewLoTrinh");
                }
            ViewBag.message = "Sai tài khoản hoặc mật khẩu";
            return View();
        }






        public ActionResult DangXuat()
        {
            Session["MaNV"] = null;
            Session["TenNV"] = null;
            Session["MatKhau"] = null;
            Session["VaiTro"] = null;

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }


     
    }
}