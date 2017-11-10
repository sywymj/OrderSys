using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using OrderSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    [ManagerAuthorize]
    public class RoleController : WeixinBaseController
    {
        //
        // GET: /Role/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ChangeRoleIndex()
        {
            MyRoleService service = new MyRoleService();
            List<RoleEntity> roles = service.GetRoleListByUser(service.CurrentUser);
            var re = roles.Select(l =>
                new DDLViewModel
                {
                    ID = SecretUtil.Encrypt(l.ID.ToString()),//需要加密
                    Title = l.FullName
                }).ToList();
            ViewBag.CurrentRoleName = service.CurrentRole.FullName;
            ViewBag.CurrentRoleID = SecretUtil.Encrypt(service.CurrentRole.ID.ToString());
            return View(re);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult DoChangeRole()
        {
            string sRoleID = SecretUtil.Decrypt(JSRequest.GetRequestUrlParm("RoleID"));
            int roleID = (int)JSValidator.ValidateInt("角色ID", sRoleID, true);

            LoginService login = new LoginService();
            login.ChangeMyCurrentRole(roleID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(ResponseType.Redict, "切换成功",data: Url.Action("ChangeRoleIndex", "Role", new { area = "weixin"})), jsonParams);
            return res;
        }
    }
}
