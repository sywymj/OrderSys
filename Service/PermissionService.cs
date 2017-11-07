﻿using CodeEngine.Framework.QueryBuilder;
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

namespace JSNet.Service
{
    public class PermissionService:BaseService
    {

        #region Resource - 资源

        public void AddResource(ResourceEntity entity)
        {
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            manager.Insert(entity);
        }

        public void EditResource(ResourceEntity entity)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldParentID, entity.ParentID));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldResourceType, entity.ResourceType));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldGroups, entity.Groups));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldTarget, entity.Target));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldNavigateUrl, entity.NavigateUrl));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldSortCode, entity.SortCode));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldIsVisible, entity.IsVisible));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldIsEnable, entity.IsEnable));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldIsPublic, entity.IsPublic));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldModifiedBy, CurrentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldModifiedOn, DateTime.Now));

            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            manager.Update(kvps, entity.ID);
        }

        public ResourceEntity GetResource(int resourceID)
        {
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            ResourceEntity entity = manager.GetSingle(resourceID);
            return entity;
        }

        public DataTable GetResourceDT(out int count)
        {
            WhereStatement where = new WhereStatement();

            ViewManager vmanager = new ViewManager("VP_Resource_Show");
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public List<ResourceEntity> GetResourceList(string parentResouceCode, bool onlyChild = true)
        {
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();

            string[] ids = GetTreeResourceIDs(parentResouceCode);
            if (ids.Length == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "ResouceCode为" + parentResouceCode));
            }

            WhereStatement where = new WhereStatement();
            where.Add(ResourceEntity.FieldID, Comparison.In, ids);

            int count = 0;
            List<ResourceEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != parentResouceCode);
            }

            return list;
        }

        private string[] GetTreeResourceIDs(string resouceCode)
        {
            string[] s = GetTreeIDs(
                "[VP_Resource]",
                "Resource_Code", resouceCode,
                "Resource_ID", "Resource_ParentID");
            return s;
        }

        #endregion

        #region PermissionItem - 操作明细

        public void AddPermissionItem(PermissionItemEntity entity)
        {
            entity.AllowEdit = (int)TrueFalse.True;
            entity.AllowDelete = (int)TrueFalse.True;
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            manager.Insert(entity);
        }

        public void EditPermissionItem(PermissionItemEntity entity)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldParentID, entity.ParentID));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldSysCategory, entity.SysCategory));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldController, entity.Controller));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldActionName, entity.ActionName));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldActionParameter, entity.ActionParameter));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldIsEnable, entity.IsEnable));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldIsPublic, entity.IsPublic));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldSortCode, entity.SortCode));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldModifiedBy, CurrentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldModifiedOn, DateTime.Now));

            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            manager.Update(kvps, entity.ID);
        }

        public PermissionItemEntity GetPermissionItem(int permissionItemID)
        {
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            PermissionItemEntity entity = manager.GetSingle(permissionItemID);
            return entity;
        }

        public DataTable GetTreePermissionItemDT(out int count, string parentPermissionItemCode = "Resource.ManagePermission")
        {
            string[] permissionItemIDs = GetTreePermissionItemIDs(parentPermissionItemCode);

            WhereStatement where = new WhereStatement();
            where.Add("PermissionItem_ID", Comparison.In, permissionItemIDs);

            ViewManager vmanager = new ViewManager("VP_PermissionItem_Show");
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public List<PermissionItemEntity> GetTreePermissionItemList(string parentPermissionItemCode, bool onlyChild = true)
        {
            string[] ids = GetTreePermissionItemIDs(parentPermissionItemCode);
            if (ids.Length == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "permissionItemCode为" + parentPermissionItemCode));
            }

            WhereStatement where = new WhereStatement();
            where.Add(PermissionItemEntity.FieldID, Comparison.In, ids);

            int count = 0;
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            List<PermissionItemEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != parentPermissionItemCode);
            }

            return list;
        }

        private string[] GetTreePermissionItemIDs(string permissionItemCode)
        {
            string[] s = GetTreeIDs(
                "[VP_PermissionItem]",
                "PermissionItem_Code", permissionItemCode,
                "PermissionItem_ID", "PermissionItem_ParentID");
            return s;
        }

        #endregion

        #region GrantItem - 配置操作明细

        public DataTable GetGrantItemsDT(string resourceCode, string resourceType, out int count)
        {
            if (!(resourceType == ResourceType.Menu.ToString()
                || resourceType == ResourceType.Button.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resourceType + "，"));
            }

            if (resourceCode.Split('.').Length < 2)
            {
                throw new JSException(JSErrMsg.ERR_CODE_ErrorFormatCode, JSErrMsg.ERR_MSG_ErrorFormatCode);
            }
            string parentPermissionItemCode = resourceCode.Split('.')[0];

            DataTable dt = GetTreePermissionItemDT(out count, parentPermissionItemCode);
            return dt;
        }

        public void GrantItem(int resourceID, int[] permissionItemIDs)
        {
            //判断资源类型是否为Menu、button，只有Menu、button才能分配资源明细
            EntityManager<ResourceEntity> resourceManager = new EntityManager<ResourceEntity>();
            ResourceEntity resource = resourceManager.GetSingle(resourceID);
            if (!(resource.ResourceType == ResourceType.Menu.ToString()
                || resource.ResourceType == ResourceType.Button.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resource.ResourceType + "，"));
            }

            EntityManager<PermissionEntity> manager = new EntityManager<PermissionEntity>();
            manager.Delete(resourceID, PermissionEntity.FieldResourceID);

            List<PermissionEntity> entitys = new List<PermissionEntity>();
            foreach (int permissionItemID in permissionItemIDs)
            {
                PermissionEntity entity = new PermissionEntity();
                entity.ResourceID = resourceID;
                entity.PermissionItemID = permissionItemID;
                entity.CreateUserId = CurrentUser.ID.ToString();
                entity.CreateBy = CurrentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        public int[] GetGrantedItemIDs(int resourceID)
        {
            WhereStatement where = new WhereStatement();
            where.Add(PermissionEntity.FieldResourceID, Comparison.Equals, resourceID);

            EntityManager<PermissionEntity> manager = new EntityManager<PermissionEntity>();
            string[] sScopeIDs = manager.GetProperties(PermissionEntity.FieldPermissionItemID, where);

            int[] scopeIDs = CommonUtil.ConvertToIntArry(sScopeIDs);
            return scopeIDs;
        }

        #endregion

        #region GrantScope - 配置资源对象

        public DataTable GetGrantScopeDT(string resourceID, string resourceTarget, out int count)
        {
            //1.0 获取资源实体
            EntityManager<ResourceEntity> resourceManager = new EntityManager<ResourceEntity>();
            ResourceEntity resource = resourceManager.GetSingle(resourceID);

            //1.1 获取资源实体所在的系统代号
            if (resource.Code.Split('.').Length < 1)
            {
                //resouceCode正确的格式 OrderSys_Data.XXXX
                throw new JSException(JSErrMsg.ERR_CODE_ErrorFormatCode, JSErrMsg.ERR_MSG_ErrorFormatCode);
            }
            string systemCode = resource.Code.Split('.')[0].Split('_')[0];

            //2.0 获取资源的数据对象
            WhereStatement where = new WhereStatement();
            where.Add("Organize_Code", Comparison.Like, systemCode + "%");

            OrderByStatement order = new OrderByStatement("Organize_Code", Sorting.Ascending);

            ViewManager vmanager = new ViewManager(resourceTarget);
            DataTable dt = vmanager.GetDataTable(where, out count, order);
            return dt;

        }

        public void GrantScope(int resourceID, string resourceTarget, int[] targetIDs, string constraint)
        {
            //判断资源类型是否为Data，只有Data才能分配资源对象
            EntityManager<ResourceEntity> resourceManager = new EntityManager<ResourceEntity>();
            ResourceEntity resource = resourceManager.GetSingle(resourceID);
            if (!(resource.ResourceType == ResourceType.Data.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resource.ResourceType + "，"));
            }

            //1.0 将资源的Target更新为对应的表/视图
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            resourceManager.Update(new KeyValuePair<string, object>(ResourceEntity.FieldTarget, resourceTarget), resource.ID, ResourceEntity.FieldID);

            //2.0 获取未修改前的资源对象ID
            int[] originalTargetIDs = GetTargetIDsByResourceID(resourceID);

            //3.0 添加新增的数据对象
            int[] arrAdd= targetIDs.Except(originalTargetIDs).ToArray();
            AddPermissionScopes(resourceID, arrAdd, constraint);

            //4.0 删除多余的数据对象
            int[] arrDel = originalTargetIDs.Except(targetIDs).ToArray();
            DeletePermissionScopes(resourceID, arrDel);
        }

        public int[] GetGrantedScopeIDs(int resourceID, string resourceTarget)
        {
            ResourceEntity resource = GetResource(resourceID);
            if (resource.Target != resourceTarget) { return new int[] { }; }

            WhereStatement where = new WhereStatement();
            where.Add(PermissionScopeEntity.FieldResourceID, Comparison.Equals, resourceID);

            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();
            string[] sItems = manager.GetProperties(PermissionScopeEntity.FieldTargetID, where);

            int[] itemIDs = CommonUtil.ConvertToIntArry(sItems);
            return itemIDs;
        }

        private int[] GetTargetIDsByResourceID(int resourceID)
        {
            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(PermissionScopeEntity.FieldResourceID, Comparison.Equals, resourceID);
            int[] targetIDs = manager.GetProperties(PermissionScopeEntity.FieldTargetID, where).ConvertToIntArry();
            return targetIDs;
        }

        private void AddPermissionScopes(int resourceID, int[] targetIDs, string constraint)
        {
            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();
            List<PermissionScopeEntity> entitys = new List<PermissionScopeEntity>();
            foreach (int targetID in targetIDs)
            {
                PermissionScopeEntity entity = new PermissionScopeEntity();
                entity.ResourceID = resourceID;
                entity.TargetID = targetID;
                entity.PermissionItemID = 2;
                entity.PermissionConstraint = constraint;
                entity.CreateUserId = CurrentUser.ID.ToString();
                entity.CreateBy = CurrentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        private void DeletePermissionScopes(int resourceID,int[] targetIDs)
        {
            if (targetIDs.Length == 0) return;

            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();

            WhereStatement where = new WhereStatement();
            where.Add(PermissionScopeEntity.FieldResourceID, Comparison.Equals, resourceID);
            where.Add(PermissionScopeEntity.FieldTargetID, Comparison.In, targetIDs);

            int[] permissionScopeIDs = manager.GetIds(where).ConvertToIntArry();

            //先删除RolePermissionScope的记录
            DeleteRolePermissionScopeByPermissionScopeIDs(permissionScopeIDs);

            manager.Delete(permissionScopeIDs);
        }

        #endregion

        #region Permission - 操作权限相关

        public bool IsPermissionAuthorizedByRole(RoleEntity role, string controllerName, string actionName)
        {
            //超级管理员权限
            if (role.ID == 1)
            {
                return true;
            }

            DataTable dt = GetAllRolesPermissions();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Role_ID"].ToString() == role.ID.ToString()
                    && dr["PermissionItem_Controller"].ToString().ToLower() == controllerName.ToLower()
                    && dr["PermissionItem_ActionName"].ToString().ToLower() == actionName.ToLower())
                {
                    return true;
                }
            }

            List<PermissionItemEntity> list = GetAllPublicPermissionItem();
            foreach (PermissionItemEntity item in list)
            {
                if (string.IsNullOrEmpty(item.Controller) || string.IsNullOrEmpty(item.ActionName))
                {
                    continue;
                }
                if (item.Controller.ToLower() == controllerName.ToLower()
                    && item.ActionName.ToLower() == actionName.ToLower())
                {
                    //item code 的格式 {系统名}_{类别}.{action}，如OrderSys_PC.Login => OrderSys
                    string sysCategory = item.Code.Split('.')[0].Split('_')[0];
                    if (role.SysCategory == sysCategory
                        && item.IsPublic == (int)TrueFalse.True)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsButtonAuthorizedByRole(string resourceCode)
        {
            return IsButtonAuthorizedByRole(CurrentRole, resourceCode);
        }

        public bool IsButtonAuthorizedByRole(RoleEntity role, string resourceCode)
        {
            //超级管理员权限
            if (role.ID == 1)
            {
                return true;
            }

            int count = 0;
            WhereStatement where1 = new WhereStatement();
            where1.Add(ResourceEntity.FieldIsPublic, Comparison.Equals, (int)TrueFalse.True);
            where1.Add(ResourceEntity.FieldCode, Comparison.Equals, resourceCode);

            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            count = manager.GetCount(where1);
            if (count > 0)
            {
                return true;
            }

            WhereStatement where = new WhereStatement();
            where.Add("Role_ID", Comparison.Equals, role.ID.ToString());
            where.Add("Resource_Code", Comparison.Equals, resourceCode);
            where.Add("Resource_IsEnable", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Resource_ResourceType", Comparison.Equals, ResourceType.Button.ToString());

            ViewManager vmanager = new ViewManager("VP_RoleResource");
            count = vmanager.GetCount(where);

            return count > 0 ? true : false;
        }

        private DataTable GetAllRolesPermissions()
        {
            // TODO 先从缓存里面拿，如果没有再从数据库拿
            WhereStatement where = new WhereStatement();
            where.Add("PermissionItem_IsEnable", Comparison.Equals, (int)TrueFalse.True);

            int count = 0;
            ViewManager vmanager = new ViewManager("VP_RolePermission");
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        private List<PermissionItemEntity> GetAllPublicPermissionItem()
        {
            WhereStatement where = new WhereStatement();
            where.Add(PermissionItemEntity.FieldIsPublic, Comparison.Equals, (int)TrueFalse.True);

            int count = 0;
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            List<PermissionItemEntity> list = manager.GetList(where, out count);
            return list;
        }

        #endregion

        #region PermissionScope - 数据权限相关

        public Dictionary<string, List<string>> GetAuthorizedScopeByRole(RoleEntity role, string scopeCode)
        {
            if (role.ID == 1)
            {
                return new Dictionary<string, List<string>>();
            }

            int count = 0;
            ViewManager vmanager = new ViewManager("VP_RoleScope");
            WhereStatement where = new WhereStatement();
            where.Add("Resource_Code", Comparison.Equals, scopeCode);
            where.Add("Role_ID", Comparison.Equals, role.ID);
            DataTable dt = vmanager.GetDataTable(where, out count);

            //填入dic
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            foreach (DataRow dr in dt.Rows)
            {
                string organizeCategory = dr["OrganizeCategory_Code"].ToString();//字段名
                //数据值，split('.')的code最后一个 OrderSys.FSWGY.TeacherDEPT，最后一个就是列值
                string organize = dr["Organize_Code"].ToString().Split('.')[dr["Organize_Code"].ToString().Split('.').Length - 1];
                if (!dic.ContainsKey(organizeCategory))
                {
                    dic.Add(organizeCategory, new List<string>());
                }
                dic[organizeCategory].Add(organize);
            }

            if (dic.Count == 0)
            {
                List<string> list = new List<string>();
                list.Add("1");
                dic.Add("0", list);//没有配置资源时，where 从句 => 0 in (1)
            }
            return dic;

        }

        /// <summary>
        /// 根据角色获取机构ID（默认递归返回获取子元素）
        /// </summary>
        /// <param name="role"></param>
        /// <param name="scopeCode"></param>
        /// <returns></returns>
        public List<string> GetAuthorizedScopeIDByRole(RoleEntity role, string scopeCode, out string scopeConstraint,bool onlyParent)
        {
            // TODO 增加是否显示所有子元素参数，默认显示所有子元素
            int count = 0;
            scopeConstraint = ""; 
            WhereStatement where = new WhereStatement();
            where.Add("Resource_Code", Comparison.Equals, scopeCode);
            where.Add("Role_ID", Comparison.Equals, role.ID);

            ViewManager vmanager = new ViewManager("VP_RoleScope");
            DataTable dt = vmanager.GetDataTable(where, out count);

            //填入list
            List<string> list = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (onlyParent)
                {
                    //dr TODO
                }
                else
                {
                    scopeConstraint = dr["PermissionScope_PermissionConstraint"].ToString();
                    string[] s = GetTreeIDs(
                        dr["Resource_Target"].ToString(),
                        "Organize_Code", dr["Organize_Code"].ToString(),
                        "ID",
                        "Organize_ID", "Organize_ParentID");//这里不正确
                    list.AddRange(s.ToList());
                }
            }


            if (list.Count == 0)
            {
                return new List<string>();
            }


            return list.Distinct<string>().ToList(); ;
        }


        #endregion

        #region 后台模块

        /// <summary>
        /// 获取对应code下一层的button
        /// </summary>
        /// <param name="role"></param>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        public DataTable GetButtons(RoleEntity role, string navigateUrl)
        {
            ViewManager vmanager = new ViewManager("VP_RoleResource");

            WhereStatement where = new WhereStatement();
            where.Add("Resource_NavigateUrl", Comparison.Equals, navigateUrl);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count);

            if (count == 0)
            {
                return new DataTable("JSNet");
            }

            WhereStatement where1 = new WhereStatement();
            where1.Add("Resource_ParentID", Comparison.Equals, dt.Rows[0]["Resource_ID"].ToString());
            where1.Add("Role_ID", Comparison.Equals, role.ID);

            int count1 = 0;
            DataTable dt1 = vmanager.GetDataTable(where1, out count1);
            return dt1;

        }

        /// <summary>
        /// 获取 指定资源code 的整个树形menu
        /// </summary>
        /// <param name="role"></param>
        /// <param name="resourceCode">资源code</param>
        /// <returns></returns>
        public DataTable GetLeftMenu(RoleEntity role, string resourceCode,bool showParent = true)
        {
            //1.0 获取已授权的树形menu
            List<string> ids = GetTreeMenuIds(role, resourceCode, showParent);
            if (ids.Count == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotGrantMenuResource, JSErrMsg.ERR_MSG_NotGrantMenuResource);
            }

            //2.0 根据id 获取详细信息
            int count = 0;
            ViewManager vmanager = new ViewManager("VP_Resource");

            WhereStatement where = new WhereStatement();
            where.Add("Resource_ID", Comparison.In, ids.ToArray());
            OrderByStatement order = new OrderByStatement("Resource_SortCode", Sorting.Ascending);

            DataTable dt = vmanager.GetDataTable(where, out count, order);
            return dt;

        }

        public List<string> GetTreeMenuIds(RoleEntity role, string resourceCode,bool showParent = true)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);
            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeParameter("Resource_Code", resourceCode) };

            string sqlQuery = @" WITH TreeMenu AS (SELECT Resource_ID AS ID
                                        FROM [VP_RoleResource] 
                                        WHERE Resource_Code = " + dbHelper.GetParameter("Resource_Code") + @"
                                        AND Role_ID = " + role.ID + @"
                                        UNION ALL
                                        SELECT ResourceTree.Resource_ID
                                            FROM [VP_RoleResource] AS ResourceTree INNER JOIN
                                                TreeMenu AS A ON A.ID = ResourceTree.Resource_ParentId
                                            WHERE Resource_ResourceType = '" + ResourceType.Menu.ToString() + @"'
                                                AND Resource_IsVisible = " + (int)TrueFalse.True + @"
                                                AND Resource_IsEnable = " + (int)TrueFalse.True + @"
                                                AND Role_ID = " + role.ID + @")
                                SELECT ID
                                    FROM TreeMenu ";
            DataTable dt = dbHelper.Fill(sqlQuery, dbParameters);
            List<string> ids =  DataTableUtil.FieldToArray(dt, "ID").ToList();
            if (ids.Count > 0 && !showParent)
            {
                ids.RemoveAt(0);
            }
            return ids;
        } 

        #endregion

        #region GrantRoleModule - 配置模块权限
        public List<ResourceEntity> GetModuleList(string parentResouceCode, bool onlyChild = true)
        {
            List<ResourceEntity> list = GetResourceList(parentResouceCode, onlyChild);
            return list.Where(l => l.ResourceType != ResourceType.Data.ToString()).ToList();
        }

        public void GrantModule(int roleID, int[] moduleIDs)
        {
            EntityManager<RoleResourceEntity> manager = new EntityManager<RoleResourceEntity>();
            manager.Delete(roleID, RoleResourceEntity.FieldRoleID);

            List<RoleResourceEntity> entitys = new List<RoleResourceEntity>();
            foreach (int moduleID in moduleIDs)
            {
                RoleResourceEntity entity = new RoleResourceEntity();
                entity.RoleID = roleID;
                entity.ResourceID = moduleID;
                entity.CreateUserId = CurrentUser.ID.ToString();
                entity.CreateBy = CurrentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        public int[] GetGrantedModuleIDs(int roleID)
        {
            WhereStatement where = new WhereStatement();
            where.Add(RoleResourceEntity.FieldRoleID, Comparison.Equals, roleID);

            EntityManager<RoleResourceEntity> manager = new EntityManager<RoleResourceEntity>();
            string[] sIDs = manager.GetProperties(RoleResourceEntity.FieldResourceID, where);

            int[] ids = CommonUtil.ConvertToIntArry(sIDs);
            return ids;
        } 

        #endregion

        #region GrantrRolePermissionScope - 配置数据权限

        public DataTable GetTreePermissionScopeDTByRole(RoleEntity role)
        {
             //如果是超级管理员，返回所有数据对象，其他则返回已分配了给该角色的数据对象（不遍历子元素）
            int count = 0;
            WhereStatement where = new WhereStatement();
            ViewManager vmanager = new ViewManager("VP_PermissionScope");
            if (role.ID == 1)
            {
                return vmanager.GetDataTable(where, out count);
            }
            string scopeConstraint = "";
            List<string> scopeIDs = GetAuthorizedScopeIDByRole(role, "OrderSys_Data.PermissionScope", out scopeConstraint);
            if (scopeIDs.Count == 0)
            {
                return new DataTable("JSNet");
            }

            where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public void GrantPermissionScope(int roleID, int[] scopeIDs)
        {
            EntityManager<RolePermissionScopeEntity> manager = new EntityManager<RolePermissionScopeEntity>();
            manager.Delete(roleID, RolePermissionScopeEntity.FieldRoleID);

            List<RolePermissionScopeEntity> entitys = new List<RolePermissionScopeEntity>();
            foreach (int scopeID in scopeIDs)
            {
                RolePermissionScopeEntity entity = new RolePermissionScopeEntity();
                entity.RoleID = roleID;
                entity.PermissionScopeID = scopeID;
                entity.CreateUserId = CurrentUser.ID.ToString();
                entity.CreateBy = CurrentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        public int[] GetGrantedPermissionScopeIDs(int roleId)
        {
            WhereStatement where = new WhereStatement();
            where.Add(RolePermissionScopeEntity.FieldRoleID, Comparison.Equals, roleId);

            EntityManager<RolePermissionScopeEntity> manager = new EntityManager<RolePermissionScopeEntity>();
            string[] sIDs = manager.GetProperties(RolePermissionScopeEntity.FieldPermissionScopeID, where);

            int[] ids = CommonUtil.ConvertToIntArry(sIDs);
            return ids;
        } 

        private string[] GetTreePermissionScopeIDs(string parentCode)
        {
            string[] s = GetTreeIDs(
                "[VP_PermissionScope]",
                "Resource_Code", parentCode,
                "Resource_ID", "Resource_ParentID");
            return s;
        } 

        //171017 new 
        /// <summary>
        /// 根据角色，获取已分配给该角色的数据资源
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public DataTable GetGrantedDataResourceByRole(RoleEntity role)
        {
            int count = 0;
            DataTable dt = new DataTable();
            ViewManager vmanager = new ViewManager("VP_PermissionScope");
            WhereStatement where = new WhereStatement();
            if (role.ID == 1)
            {
                //超级用户默认获取所有的资源对象
                dt = vmanager.GetDataTable(where, out count);
                return dt;
            }

            string scopeConstraint = "";
            List<string> scopeIDs = GetAuthorizedScopeIDByRole(role, "OrderSys_Data.PermissionScope", out scopeConstraint);
            where.Add("Organize_ID", Comparison.In, scopeIDs.ToArray());

            dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        /// <summary>
        /// 根据角色，获取已分配给该角色的数据资源ID
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private string[] GetGrantedDataResourceIDsByRole(RoleEntity role)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);
            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeParameter("Role_ID", role.ID) };

            string sqlQuery = @"select Resource_ID from [DB_OrderSys].[dbo].[VP_RoleScope]
                    where Role_ID = " + dbHelper.GetParameter("Role_ID") + @"
                    group by Resource_ID";
            DataTable dt = dbHelper.Fill(sqlQuery, dbParameters);
            return DataTableUtil.FieldToArray(dt, "ID");
        }

        private void DeleteRolePermissionScopeByPermissionScopeIDs(int[] permissionScopeIDs)
        {
            if (permissionScopeIDs.Length == 0) return;

            WhereStatement where = new WhereStatement();
            where.Add(RolePermissionScopeEntity.FieldPermissionScopeID, Comparison.In, permissionScopeIDs);

            EntityManager<RolePermissionScopeEntity> manager = new EntityManager<RolePermissionScopeEntity>();
            manager.Delete(where);
        }

        #endregion

        public bool ChkResourceCodeExist(string resourceCode, string resourceID)
        {
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            bool b = ChkExist<ResourceEntity>(
                manager,
                ResourceEntity.FieldCode, resourceCode,
                ResourceEntity.FieldID, resourceID);
            return b;
        }

        public bool ChkPermissionItemCodeExist(string permissionItemCode, string permissionItemID)
        {
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            bool b = ChkExist<PermissionItemEntity>(
                manager,
                PermissionItemEntity.FieldCode, permissionItemCode,
                PermissionItemEntity.FieldID, permissionItemID);
            return b;
        }

    }
}
