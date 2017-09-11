using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        //
        // GET: /User/
        UserService service = new UserService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Add()
        {
            string sRoleIDs = JSRequest.GetRequestFormParm("roleIDs");
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewUser viewModel = FastJSON.JSON.ToObject<ViewUser>(s);

            //TODO 数据验证。
            int[] roleIDs = JSValidator.ValidateStrings("角色ID", sRoleIDs, true);
            

            UserEntity user = new UserEntity();
            StaffEntity staff = new StaffEntity();
            List<RoleEntity> roles = new List<RoleEntity>();

            viewModel.CopyTo(user);
            viewModel.Staff.CopyTo(staff);

            service.AddUser(user, staff, roleIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }

        [HttpPost]
        public string Edit()
        {
            string sRoleIDs = JSRequest.GetRequestFormParm("roleIDs");
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewUser viewModel = FastJSON.JSON.ToObject<ViewUser>(s);

            //TODO 数据验证。
            int[] roleIDs = JSValidator.ValidateStrings("角色ID", sRoleIDs, true);


            UserEntity user = new UserEntity();
            StaffEntity staff = new StaffEntity();
            List<RoleEntity> roles = new List<RoleEntity>();

            viewModel.CopyTo(user);
            viewModel.Staff.CopyTo(staff);

            service.EditUser(user, staff, roleIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }

        [HttpGet]
        public string GetSingle(int userID)
        {
            ViewUser viewModel = new ViewUser();
            MyRoleService roleService = new MyRoleService();

            UserEntity user = service.GetUser(userID);
            StaffEntity staff = service.GetStaff((int)user.ID);
            int[] roleIDs = roleService.GetRoleIDs((int)user.ID);

            user.CopyTo(viewModel);
            viewModel.Staff = staff;
            viewModel.RoleIDs = roleIDs;

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        }

        [HttpGet]
        public string VerifyUserName(string userName)
        {
            bool re = false;
            
            if (service.ChkUserNameExist(userName))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
            
        }
    }
}
