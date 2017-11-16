using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using JSNet.Manager;
using JSNet.Model;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace JSNet.Service
{
    public class UserService : BaseService
    {

        #region Current
        public UserEntity GetCurrentUser()
        {
            int uid = 0;
            if (!Int32.TryParse(SecretUtil.Decrypt(JSRequest.GetCookie("UID", true)), out uid))
            {
                return null;
            }

            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity user = manager.GetSingle(uid);
            return user;
        }

        public UserEntity GetCurrentVXUser()
        {
            string openID = JSRequest.GetCookie("OpenID", true);
            if (string.IsNullOrEmpty(openID))
            {
                return null;
            }

            UserEntity user = GetUser(openID);
            return user;
        }

        public StaffEntity GetCurrentStaff()
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity staff = manager.GetSingle(CurrentUser.ID, StaffEntity.FieldUserID);
            return staff;
        }

        #endregion

        #region User
        public void AddUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            //添加user
            if (string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = "123456";// TODO 加盐转MD5加密保存
            }
            entity.OpenID = null;
            entity.AddedVXUser = (int)TrueFalse.False;
            entity.IsEnable = (int)TrueFalse.True;
            entity.OrganizeID = staff.OrganizeID;
            entity.DeletionStateCode = (int)TrueFalse.False;
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;
            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            string userID = userManager.Insert(entity);

            //添加staff
            staff.UserID = Convert.ToInt32(userID);
            string staffID = AddStaff(staff);

            //添加role-user-rel
            MyRoleService roleService = new MyRoleService();
            roleService.GrantRole(Convert.ToInt32(userID), roleIDs);

            //添加微信账户
            KawuService kawuService = new KawuService();
            kawuService.AddWeixinUser(staff.Tel, entity.UserName);
            userManager.Update(new KeyValuePair<string, object>(UserEntity.FieldAddedVXUser, (int)TrueFalse.True), Convert.ToInt32(userID));

        }

        public void AddWeiXinUser(int[] userIDs)
        {
            List<UserEntity> users = GetUserList(userIDs);

            KawuService kawuService = new KawuService();
            foreach(UserEntity user in users)
            {
                if (user.AddedVXUser == (int)TrueFalse.True) {
                    throw new JSException(JSErrMsg.ERR_CODE_AddedVXUser, JSErrMsg.ERR_MSG_AddedVXUser);
                }

                //增加微信用户
                StaffEntity staff = GetStaffByUserID((int)user.ID);
                kawuService.AddWeixinUser(staff.Tel, user.UserName);

                //修改用户状态为 已增加微信用户
                user.AddedVXUser = (int)TrueFalse.True;
                EditUserVXUserStatus(user);
            }
        }

        public void EditUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            //0.0 修改卡务对应的手机号码
            StaffEntity originalStaff = GetStaffByUserID((int)entity.ID);
            KawuService kawuService = new KawuService();
            kawuService.ChangeUserData(originalStaff.Tel, staff.Tel, entity.UserName);

            //1.0 修改用户信息
            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> userTargetKVPs = new List<KeyValuePair<string, object>>();
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldUserName, entity.UserName));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldPassword, entity.Password));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldIsLogin, entity.IsLogin));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldOrganizeID, staff.OrganizeID));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedBy, CurrentUser.UserName));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedOn, DateTime.Now));
            userManager.Update(userTargetKVPs, entity.ID);

            //2.0 修改员工信息
            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            StaffEntity staff1 = staffManager.GetSingle(entity.ID, StaffEntity.FieldUserID);
            List<KeyValuePair<string, object>> staffTargetKVPs = new List<KeyValuePair<string, object>>();
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldName, staff.Name));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldTel, staff.Tel));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldAddr, staff.Addr));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldSex, staff.Sex));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldOrganizeID, staff.OrganizeID));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldIsOnJob, staff.IsOnJob));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedBy, CurrentUser.UserName));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedOn, DateTime.Now));
            staffManager.Update(staffTargetKVPs, staff1.ID);

            //3.0 删除role-user-rel关系
            EntityManager<UserRoleEntity> userRoleManager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID, Comparison.Equals, entity.ID);
            userRoleManager.Delete(where);

            //3.1 增加role-user-rel关系
            foreach (int roleID in roleIDs)
            {
                UserRoleEntity userRole = new UserRoleEntity()
                {
                    RoleID = roleID,
                    UserID = entity.ID,
                    CreateUserId = CurrentUser.ID.ToString(),
                    CreateBy = CurrentUser.UserName,
                    CreateOn = DateTime.Now,
                };
                userRoleManager.Insert(userRole);
            }


        }

        public void EditUserOpenID(string tel,string openID)
        {
            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            StaffEntity staff = staffManager.GetSingle(tel, StaffEntity.FieldTel);
            if (staff == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_WrongTel, JSErrMsg.ERR_MSG_WrongTel);
            }

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(UserEntity.FieldOpenID, openID));
            userManager.Update(kvps, staff.UserID);
        }

        public void EditUserVXUserStatus(UserEntity user)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(UserEntity.FieldAddedVXUser, user.AddedVXUser));

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            userManager.Update(kvps, user.ID);
        }

        public void ClearUserOpenID(string openID)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(UserEntity.FieldOpenID, ""));

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            userManager.Update(kvps, openID, UserEntity.FieldOpenID);
        }

        public UserEntity GetUser(int userID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity entity = manager.GetSingle(userID);
            return entity;
        }

        public UserEntity GetUser(string openID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity entity = manager.GetSingle(openID, UserEntity.FieldOpenID);
            return entity;
        }

        public UserEntity GetUser(string userName, string pwd)
        {
            int count = 0;

            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldUserName, Comparison.Equals, userName);
            where.Add(UserEntity.FieldPassword, Comparison.Equals, pwd);
            
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            List<UserEntity> ls = manager.GetList(where, out count);
            return ls.FirstOrDefault();
        }

        public List<UserEntity> GetUserList(int[] userIDs)
        {
            int count = 0;
            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldID,Comparison.In,userIDs);

            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            List<UserEntity> list = manager.GetList(where, out count);
            return list;
        }

        public List<UserEntity> GetUserList()
        {
            int count = 0;
            WhereStatement where = new WhereStatement();

            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            List<UserEntity> list = manager.GetList(where, out count);
            return list;
        }

        public DataTable GetUserDTForShow(RoleEntity role, Paging paging, out int count)
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.User", out scopeConstraint);

            WhereStatement where = new WhereStatement();
            where.Add("Staff_IsEnable", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Staff_IsOnJob", Comparison.Equals, (int)TrueFalse.True);
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //非超级管理员，不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            ViewManager vmanager = new ViewManager("VS_User_Show");
            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }

        /// <summary>
        /// 获取该角色下的用户
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public DataTable GetUserDT(int[] roleIDs)
        {
            int count = 0;
            WhereStatement where = new WhereStatement();
            where.Add("Staff_IsEnable", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Staff_IsOnJob", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Role_ID", Comparison.In, roleIDs);

            ViewManager vmanager = new ViewManager("VP_UserRoleStaff");
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public void Import(DataTable dt,out string result)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_ImportNoContent, JSErrMsg.ERR_MSG_ImportNoContent);
            }

            string[] columns = new string[] { "账号", "密码", "姓名", "手机号码", "地址", "性别", "部门编码" };
            foreach (string col in columns)
            {
                if (!dt.Columns.Contains(col)) throw new JSException(JSErrMsg.ERR_CODE_ImportColError, string.Format(JSErrMsg.ERR_MSG_ImportColError, col));
            }
            dt.Columns.Add("处理结果", typeof(string));

            List<UserEntity> users = GetUserList();
            List<StaffEntity> staffs = GetStaffs();
            List<OrganizeEntity> organizes = new OrganizeService().GetTreeOrganizeListByUser(CurrentUser);
            Dictionary<SexType,string> sexType = EnumExtensions.ConvertToEnumDic<SexType>();

            Dictionary<UserEntity, StaffEntity> importDic = new Dictionary<UserEntity, StaffEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                #region 赋值
                string userName = dr["账号"].ToString().Trim();
                string password = dr["密码"].ToString().Trim();
                string name = dr["姓名"].ToString().Trim();
                string tel = dr["手机号码"].ToString().Trim();
                string addr = dr["地址"].ToString().Trim();
                string sex = dr["性别"].ToString().Trim();
                string orgCode = dr["部门编码"].ToString().Trim();
                #endregion

                #region 验证

                if (string.IsNullOrEmpty(userName))
                {
                    dr["处理结果"] = "账号不能为空！"; continue;
                }
                if (string.IsNullOrEmpty(tel))
                {
                    dr["处理结果"] = "手机号码不能为空！"; continue;
                }
                if (string.IsNullOrEmpty(name))
                {
                    dr["处理结果"] = "姓名不能为空！"; continue;
                }
                if (string.IsNullOrEmpty(sex))
                {
                    dr["处理结果"] = "性别不能为空！"; continue;
                }
                if (string.IsNullOrEmpty(orgCode))
                {
                    dr["处理结果"] = "部门编码不能为空！"; continue;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(tel, @"^1[3,4,5,7,8]\d{9}$"))
                {
                    dr["处理结果"] = "手机号码格式有误！"; continue;
                }
                if (!sexType.ContainsValue(sex))
                {
                    dr["处理结果"] = "性别格式有误！"; continue;
                }
                if (users.Count(x => x.UserName == userName) > 0)
                {
                    dr["处理结果"] = "账号已存在！"; continue;
                }
                if (staffs.Count(x => x.Tel == tel) > 0)
                {
                    dr["处理结果"] = "手机号码已存在！"; continue;
                }
                if (organizes.Count(x => x.Code == orgCode) == 0)
                {
                    dr["处理结果"] = "部门编号不正确！"; continue;
                }
                #endregion

                UserEntity user = new UserEntity();
                user.UserName = userName;
                user.Password = password;
                user.IsLogin = (int)TrueFalse.True;

                StaffEntity staff = new StaffEntity();
                staff.Name = name;
                staff.Tel = tel;
                staff.Addr = addr;
                staff.Sex = (int)sexType.Where(d => d.Value == sex).FirstOrDefault().Key;
                staff.OrganizeID = organizes.Where(x => x.Code == orgCode).FirstOrDefault().ID;
                staff.IsOnJob = (int)TrueFalse.True;

                try
                {
                    AddUser(user, staff, new int[0]);
                }
                catch(Exception e)
                {
                    dr["处理结果"] = "处理失败！" + e.Message.Replace("\r\n",""); continue;
                }
                dr["处理结果"] = "处理成功！";
                users.Add(user);
                staffs.Add(staff);
            }

            ExportService exportService = new ExportService();
            string localpath = "";
            string webpath = exportService.GetExportFolderWebPath("user", out localpath);
            string fileName = exportService.GetFileName() + ".csv";

            BaseExportCSV.ExportCSV(dt, localpath + fileName);
            result = webpath + fileName;
        }

        #endregion

        #region Staff

        public string AddStaff(StaffEntity entity)
        {
            entity.IsEnable = (int)TrueFalse.True;
            entity.DeletionStateCode = (int)TrueFalse.False;
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<StaffEntity> manager =  new EntityManager<StaffEntity>();
            return manager.Insert(entity);
        }

        public StaffEntity GetStaffByUserID(int userID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity entity = manager.GetSingle(userID, StaffEntity.FieldUserID);
            return entity;
        }

        public StaffEntity GetStaff(int staffID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity entity = manager.GetSingle(staffID, StaffEntity.FieldID);
            return entity;
        }

        public List<StaffEntity> GetStaffs()
        {
            int count = 0;
            WhereStatement where = new WhereStatement();

            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            List<StaffEntity> list = manager.GetList(where, out count);
            return list;
        }

        public List<StaffEntity> GetWorkingStaffsByRole(RoleEntity role)
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.WorkingStaff", out scopeConstraint);

            WhereStatement where = new WhereStatement();
            where.Add(StaffEntity.FieldIsEnable, Comparison.Equals, (int)TrueFalse.True);
            where.Add(StaffEntity.FieldIsOnJob, Comparison.Equals, (int)TrueFalse.True);
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //默认不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            int count = 0;
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            List<StaffEntity> list = manager.GetList(where, out count);
            return list;
        }
        #endregion

        public bool ChkUserNameExist(string userName, string userID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();

            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldUserName, Comparison.Equals, userName);
            if (!string.IsNullOrEmpty(userID))
            {
                //编辑时
                where.Add(UserEntity.FieldID, Comparison.NotEquals, userID);
            }

            bool b = manager.Exists(where);
            return b;
        }

        public bool ValidateTel(string tel, string userID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();

            WhereStatement where = new WhereStatement();
            where.Add(StaffEntity.FieldTel, Comparison.Equals, tel);
            if (!string.IsNullOrEmpty(userID))
            {
                //编辑时
                StaffEntity staff = GetStaffByUserID(Convert.ToInt32(userID));
                where.Add(StaffEntity.FieldID, Comparison.NotEquals, staff.ID);
            }

            bool b = manager.Exists(where);
            return b;
        }
    }
}
