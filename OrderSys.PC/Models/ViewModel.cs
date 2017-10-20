using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OrderSys.Admin
{
    public abstract class BaseViewDDL
    {
        public string ID { get; set; }
        public string Title { get; set; }
    }
    public abstract class BaseTreeViewDDL : BaseViewDDL
    {
        public string ParentID { get; set; }
    }

    //Tree DDL
    public class ViewOrganizeDDL : BaseTreeViewDDL { }
    public class ViewResourceDDL : BaseTreeViewDDL { }
    public class ViewPermissionItemDDL : BaseTreeViewDDL { }

    //普通 DDL
    public class ViewRoleDDL : BaseViewDDL { }
    public class ViewResourceTypeDDL : BaseViewDDL { }
    public class ViewResourceCodeDDL : BaseViewDDL { }
    public class ViewSysCategoryDDL : BaseViewDDL { }
    public class ViewOrganizeCategoryDDL : BaseViewDDL { }
    public class ViewOrganizeCodeDDL : BaseViewDDL { }

    //ViewModel
    public class ViewUser : UserEntity
    {
        public StaffEntity Staff { get; set; }

        public string RoleIDs { get; set; }
    }
    public class ViewResource : ResourceEntity
    {
        public string permissionItemIDs { get; set; }
    }
    public class ViewPermissionItem : PermissionItemEntity { }
    public class ViewOrganize : OrganizeEntity { }
    public class ViewOrganizeCategory : OrganizeCategoryEntity { }

    public class ViewRole : RoleEntity 
    {
        public List<RolePermissionScopeEntity> RolePermissionScopes { get; set; }
        public List<RoleResourceEntity> RoleResource { get; set; }
        public string GrantedScopeIDs { get; set; }
        public string GrantedResourceIDs { get; set; }
    
    }

    public class ViewDataResource
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Title { get; set; }
        public List<ViewPermissionScopeEntity> Expands { get; set; }
    }

    public class ViewPermissionScopeEntity
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public bool Checked { get; set; }
    }

    //ListData ViewModel
    public class DataTableData
    {
        public DataTableData(DataTable dt)
        {
            this.DT = dt;
        }
        public DataTableData(DataTable dt, int count)
            : this(dt)
        {
            this.Count = count;
        }
        public DataTable DT { get; set; }
        public int Count { get; set; }
    }

    public class ListData<T>
    {
        public ListData(List<T> list)
        {
            this.List = list;
        }

        public ListData(List<T> list, int count)
        {
            this.List = list;
            this.Count = count;
        }
        public List<T> List { get; set; }
        public int Count { get; set; }
    }
}