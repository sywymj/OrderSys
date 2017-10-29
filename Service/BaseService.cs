using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using FastJSON;
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
    public class BaseService
    {
        private UserEntity _currentUser;
        public UserEntity CurrentUser
        {
            get
            {
                if(_currentUser==null)
                {
                    UserService userService = new UserService();
                    //先拿openid
                    UserEntity user = null;
                    user = userService.GetCurrentVXUser();
                    if (user == null)
                    {
                        // 再拿uid
                        user = userService.GetCurrentUser();
                    }
                    if (user == null)
                    {
                        throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
                    }
                    _currentUser = user;
                }
                return _currentUser;
            }
        }

        private RoleEntity _currentRole;
        public RoleEntity CurrentRole
        {
            get
            {
                if (_currentUser == null)
                {
                    MyRoleService roleService = new MyRoleService();
                    RoleEntity role = roleService.GetCurrentRole();
                    if (role == null)
                    {
                        throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
                    }
                    _currentRole = role;
                }
                return _currentRole;
            }
        }

        private StaffEntity _currentStaff;
        public StaffEntity CurrentStaff
        {
            get
            {
                if(_currentStaff==null)
                {
                    UserService userService = new UserService();
                    StaffEntity staff = userService.GetCurrentStaff();
                    if (staff == null)
                    {
                        throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
                    }
                    _currentStaff = staff;
                }
                return _currentStaff;
            }
        }

        protected JSONParameters jsonParams = new JSONParameters()
        {
            UseUTCDateTime = false,
            UsingGlobalTypes = false,
            UseExtensions = false,
        };

        public Sorting ConvertToSort(string sortOrder)
        {
            switch (sortOrder.ToLower())
            {
                case "asc":
                    return Sorting.Ascending;
                case "desc":
                    return Sorting.Descending;
                case "":
                    return Sorting.Ascending;
                default:
                    throw new JSException(string.Format("sortOrder值“{0}”不合法！", sortOrder));
            }
        }

        /// <summary>
        /// 检查字段值是否已存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="manager"></param>
        /// <param name="chkField">所在字段名称</param>
        /// <param name="chkValue">字段值</param>
        /// <param name="idFiled">ID字段（用于在修改内容时判断）</param>
        /// <param name="idValue">ID值（用于在修改内容时判断）</param>
        /// <returns></returns>
        public bool ChkExist<T>(EntityManager<T> manager, string chkField, string chkValue, string idField, string idValue)
            where T : BaseEntity, new()
        {
            WhereStatement where = new WhereStatement();
            where.Add(chkField, Comparison.Equals, chkValue);
            if (!string.IsNullOrEmpty(idValue))
            {
                //编辑时
                where.Add(idField, Comparison.NotEquals, idValue);
            }

            bool b = manager.Exists(where);
            return b;
        }

        /// <summary>
        /// 获取 树形ID数组
        /// </summary>
        /// <param name="tableName">表、视图名称</param>
        /// <param name="filterField">筛选父元素的字段</param>
        /// <param name="filterValue">筛选父元素的字段值</param>
        /// <param name="idField">ID字段</param>
        /// <param name="parentIDField">parentID字段</param>
        /// <returns></returns>
        public string[] GetTreeIDs(string tableName,string filterField, string filterValue,string idField,string parentIDField)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);
            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeParameter(filterField, filterValue) };

            string sqlQuery = @" WITH Tree1 AS (
                                    SELECT " + idField + @" AS ID
                                        FROM " + tableName + @" 
                                        WHERE " + filterField + @"  = " + dbHelper.GetParameter(filterField) + @"
                                    UNION ALL
                                    SELECT Tree." + idField + @"
                                        FROM " + tableName + @" AS Tree INNER JOIN
                                        Tree1 AS A ON A.ID = Tree." + parentIDField + @")
                                SELECT ID
                                    FROM Tree1 ";
            DataTable dt = dbHelper.Fill(sqlQuery, dbParameters);
            return DataTableUtil.FieldToArray(dt, "ID");

        }
    }
}
