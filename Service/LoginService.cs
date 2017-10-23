using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JSNet.Service
{
    public class LoginService:BaseService
    {
        

        #region Login
        public void Login(string userName, string pwd)
        {
            UserService userService = new UserService();
            UserEntity user = userService.GetUser(userName, pwd);
            if (user == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_WrongPwd);
            }
            if (user.IsEnable == (int)TrueFalse.False)
            {
                throw new JSException(JSErrMsg.ERR_MSG_UserUnable);
            }
            if (user.IsLogin == (int)TrueFalse.False)
            {
                throw new JSException(JSErrMsg.ERR_MSG_NotAllowLogin);
            }

            MyRoleService roleService = new MyRoleService();
            RoleEntity role = roleService.GetGrantedRole(user);
            if (role == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_NotGrantRole);
            }

            JSResponse.WriteCookie("UID", SecretUtil.Encrypt(user.ID.ToString()), 120);
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()), 120);
            JSResponse.WriteCookie("OpenID", user.OpenID, 120);
            JSResponse.WriteCookie("AdminName", user.UserName, 120);
            JSResponse.WriteCookie("AdminPwd", user.Password, 120);
        }

        public void VXLogin(string openID)
        {
            UserService userService = new UserService();
            UserEntity user = userService.GetUser(openID);
            if (user == null)
            {
                //没登录直接报错，返回json前端处理
                throw new HttpException(401, JSErrMsg.ERR_MSG_WrongOpenID);
            }
            if (user.IsEnable == (int)TrueFalse.False)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_UserUnable);
            }
            if (user.IsLogin == (int)TrueFalse.False)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_NotAllowLogin);
            }

            string rid = SecretUtil.Decrypt(JSRequest.GetCookie("RID", true));
            MyRoleService roleService = new MyRoleService();
            RoleEntity role = roleService.GetRole(openID);

            //TODO 从cookie记录之前登录的角色：（有bug，需要判断rid是否为当前用户的，否则会存在跨角色访问）
            //RoleEntity role = string.IsNullOrEmpty(rid) ?
            //    roleService.GetRole(openID) :
            //    roleService.GetRole(Convert.ToInt32(rid));

            if (role == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_LoginOvertime);
            }

            //写入cookie
            JSResponse.WriteCookie("OpenID", user.OpenID, 120);
            //JSResponse.WriteCookie("UID", SecretUtil.Encrypt(user.ID.ToString()), 120);
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()), 120);
        }

        public void Logout()
        {
            JSResponse.WriteCookie("UID", "");
            JSResponse.WriteCookie("RID", "");
            JSResponse.WriteCookie("OpenID", "");
            JSResponse.WriteCookie("AdminName", "");
            JSResponse.WriteCookie("AdminPwd", "");
        }

        public void VXLogout(string openID)
        {
            UserService userService = new UserService();
            userService.ClearUserOpenID(openID);

            JSResponse.WriteCookie("OpenID", "");
            JSResponse.WriteCookie("RID", "");
        }

        public void ChkLogin(out UserEntity user, out RoleEntity role)
        {
            string rid = SecretUtil.Decrypt(JSRequest.GetCookie("RID", true));
            string uid = SecretUtil.Decrypt(JSRequest.GetCookie("UID", true));

            if (string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(rid))
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
            }

            UserService userService = new UserService();
            user = userService.GetUser(Convert.ToInt32(uid));
            if (user == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
            }
            if (user.IsEnable == (int)TrueFalse.False)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_UserUnable);
            }
            if (user.IsLogin == (int)TrueFalse.False)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_NotAllowLogin);
            }
            if (user.ID.ToString() != uid)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_UserUnable);
            }

            MyRoleService roleService = new MyRoleService();
            role = roleService.GetRole(Convert.ToInt32(rid));
            if (role == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_NotGrantRole);
            }

            //写入cookie
            JSResponse.WriteCookie("UID", SecretUtil.Encrypt(uid), 120);
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(rid), 120);
        }

        public void ChkVXLogin(out UserEntity user, out RoleEntity role)
        {
            string openID = JSRequest.GetCookie("OpenID", true);
            string rid = SecretUtil.Decrypt(JSRequest.GetCookie("RID", true));

            if (string.IsNullOrEmpty(openID)
                || string.IsNullOrEmpty(rid))
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
            }

            UserService userService = new UserService();
            user = userService.GetUser(openID);
            if (user == null)
            {
                //没登录直接报错，返回json前端处理
                throw new HttpException(401, JSErrMsg.ERR_MSG_WrongOpenID);
            }
            if (user.IsEnable == (int)TrueFalse.False)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_UserUnable);
            }
            if (user.IsLogin == (int)TrueFalse.False)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_NotAllowLogin);
            }

            //切换用户时，


            MyRoleService roleService = new MyRoleService();
            role = roleService.GetRole(Convert.ToInt32(rid));

            if (role == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_NotGrantRole);
            }

            //写入cookie
            JSResponse.WriteCookie("OpenID", user.OpenID, 120);
            //JSResponse.WriteCookie("UID", SecretUtil.Encrypt(user.ID.ToString()), 120);
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()), 120);
        }

        #endregion

        public void ChangeMyCurrentRole(int roleID)
        {
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(roleID.ToString()), 120);
        }
    }
}
