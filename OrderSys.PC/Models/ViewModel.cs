using JSNet.Model;
using System;
using System.Collections.Generic;
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
}