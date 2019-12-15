using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NienLuan2.Models
{
  
    public class KiemTraQuyenAttribute : AuthorizeAttribute
    {
        public string vaitro { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Lấy thông tin người dùng lưu vào session
            NHANVIEN nv = (NHANVIEN)HttpContext.Current.Session["online"];
            if (nv != null)
            {
                //Lay duoc ma vai tro
                string RoleID = nv.VT_MA;
                VAITRO vt = null;
                    try {
                    vt = Common.Instance.VAITROes.Where(p => p.VT_MA != null && p.VT_MA.Equals(RoleID) &&  p.VT_MA.Equals(vaitro)).First<VAITRO>();
                    if(vt != null && vt.VT_MA.Equals(RoleID))
                    {
                        return true;
                    }
                } catch {}
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/DangNhap/LoiPhanQuyen.cshtml"
            };
        }
    }
}
