using System;
using System.Collections.Generic;
using System.Data;

namespace JSNet.Manager
{
    using BaseSys;
    using CodeEngine.Framework.QueryBuilder;
    using CodeEngine.Framework.QueryBuilder.Enums;
    using JSNet.Utilities;
    using JSNet.DbUtilities;
    using JSNet.Model;
    using System.Data.Common;

    public partial class EntityManager<T>:BaseManager
        where T : BaseEntity, new()
    {
        protected IEntity<T> _iEntity = null;

        /// <summary>
        /// 默认使用 CenterDbType 数据库类型，CenterDbConnection 链接字符串
        /// </summary>
        public EntityManager()
            :base()
        {
            this._iEntity = (IEntity<T>)new T();
            this.CurrentTableName = this._iEntity.TableName;
            
        }

        public EntityManager(IDbHelper dbHelper)
            : base(dbHelper)
        {

        }

        public EntityManager(string tableName)
            : base(tableName)
        {

        }

        public EntityManager(IDbHelper dbHelper, string tableName)
            : base(dbHelper,tableName)
        {

        }

        public IDbDataParameter MakeInParam(string paramName, string dbType, int size, object value)
        {

            return this.DbHelper.MakeInParam(paramName, dbType, size, value);
        }

        public IDbDataParameter MakeOutParam(string paramName,string dbType, int size)
        {

            return this.DbHelper.MakeOutParam(paramName, dbType, size);
        }

        public DataTable GetFromProcedure(string procedureName,IDbDataParameter[] dbParameters)
        {
            return this.DbHelper.ExecuteProcedureForDataTable(procedureName, this.CurrentTableName, dbParameters);
        }

        public DataTable GetFromProcedure(string procedureName, IDbDataParameter[] dbParameters, out List<IDbDataParameter> outDbParameters)
        {
            return this.DbHelper.ExecuteProcedureForDataTable(procedureName, this.CurrentTableName, dbParameters,out outDbParameters);
        }

        public DataTable GetFromProcedure(string procedureName, string id)
        {
            string[] names = new string[1];
            object[] values = new object[1];
            names[0] = this._iEntity.PrimaryKey;
            values[0] = id;
            return this.DbHelper.ExecuteProcedureForDataTable(procedureName, this.CurrentTableName, this.DbHelper.MakeParameters(names, values));
        }

        public T ToObject(DataRow dr)
        {
            return _iEntity.GetFrom(dr);
        }

        public virtual List<T> ToList(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = ToObject(dr);
                
                list.Add(t);
            }
            return list;
        }

    }
}