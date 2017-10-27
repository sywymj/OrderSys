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

            RoleEntity role = GetLoginRole(user);
            if (role == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_NotGrantRole);
            }

            JSResponse.WriteCookie("UID", SecretUtil.Encrypt(user.ID.ToString()));
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()));
            JSResponse.WriteCookie("OpenID", user.OpenID);
            JSResponse.WriteCookie("AdminName", user.UserName);
            JSResponse.WriteCookie("AdminPwd", user.Password);
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

            RoleEntity role = GetLoginRole(user);
            if (role == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_NotGrantRole);
            }
            //写入cookie
            JSResponse.WriteCookie("OpenID", user.OpenID);
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()));
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

        /// <summary>
        /// 获取登陆时的角色对象
        /// </summary>
        /// <param name="user"></param>
        /// <param name="defatult">若cookie里面的role正确，是否返回当前用户的第一个角色</param>
        /// <returns></returns>
        public RoleEntity GetLoginRole(UserEntity user, bool defatult = true)
        {
            RoleEntity role = null;
            MyRoleService roleService = new MyRoleService();
            List<RoleEntity> roles = roleService.GetRoleListByUser(user);
            if (roles.Count == 0)
            {
                return null;
            }

            int rid = 0;
            if (!Int32.TryParse(SecretUtil.Decrypt(JSRequest.GetCookie("RID", true)), out rid))
            {
                if (defatult)
                    return roles[0];
                else
                    return null;
            }

            if (roles.Count(r => r.ID == rid) == 0)
            {
                //cookie里的rid不属于当前用户。
                if (defatult)
                    return roles[0];
                else
                    return null;
            }
            
            role = roles.FirstOrDefault(r => r.ID == rid);
            return role;
        }

        public void ChkLogin(out UserEntity user, out RoleEntity role)
        {
            string uid = SecretUtil.Decrypt(JSRequest.GetCookie("UID", true));

            if (string.IsNullOrEmpty(uid))
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

            role = GetLoginRole(user, false);
            if (role == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_UserEdited);
            }

            //写入cookie
            JSResponse.WriteCookie("UID", SecretUtil.Encrypt(uid));
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()));
        }

        public void ChkVXLogin(out UserEntity user, out RoleEntity role)
        {
            string openID = JSRequest.GetCookie("OpenID", true);
            if (string.IsNullOrEmpty(openID))
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

            role = GetLoginRole(user, false);
            if (role == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_UserEdited);
            }

            //写入cookie
            JSResponse.WriteCookie("OpenID", user.OpenID);
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(role.ID.ToString()));
        }

        #endregion

        public void ChangeMyCurrentRole(int roleID)
        {
            JSResponse.WriteCookie("RID", SecretUtil.Encrypt(roleID.ToString()));
        }
    }
}
