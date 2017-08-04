using JSNet.DbUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.Manager
{
    public class ViewManager:BaseManager
    {
        public ViewManager()
            :base()
        {
        }

        public ViewManager(IDbHelper dbHelper)
            : base(dbHelper)
        {

        }

        public ViewManager(string tableName)
            : base(tableName)
        {

        }

        public ViewManager(IDbHelper dbHelper, string tableName)
            : base(dbHelper,tableName)
        {

        }
    }
}
