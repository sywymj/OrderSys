using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSys.Admin
{
    public class ViewOrganizeDDL
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Title { get; set; }
    }

    public class ViewUser:UserEntity
    {
        public StaffEntity Staff { get; set; }
        public List<RoleEntity> Roles { get; set; }
 
    }
}