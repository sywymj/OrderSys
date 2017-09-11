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
        public string ParentID { get; set; }
        public string Title { get; set; }
    }

    public class ViewOrganizeDDL:BaseViewDDL
    {
    }

    public class ViewRoleDDL : BaseViewDDL
    {
    }

    public class ViewUser:UserEntity
    {
        public StaffEntity Staff { get; set; }

        public int[] RoleIDs { get; set; }
    }
}