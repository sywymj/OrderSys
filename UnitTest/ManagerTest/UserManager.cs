using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSNet.Utilities;
using JSNet.DbUtilities;
using JSNet.Manager;

namespace UnitTest
{
    public class UserManager : EntityManager<UserEntity>
    {
        public UserManager() : base() { }

        public UserManager(IDbHelper dbHelper) : base(dbHelper) { }

        public UserManager(IDbHelper dbHelper, string tableName) : base(dbHelper,tableName) { }
    }
}
