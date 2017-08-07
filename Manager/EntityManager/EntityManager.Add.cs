using System;
using System.Collections.Generic;
using System.Data;

namespace JSNet.Manager
{
    using CodeEngine.Framework.QueryBuilder;
    using CodeEngine.Framework.QueryBuilder.Enums;
    using JSNet.Utilities;
    using JSNet.DbUtilities;
    using JSNet.Model;
    using System.Data.Common;

    public partial class EntityManager<T>
        where T : BaseEntity, new()
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体类型</param>
        /// <param name="returnId">返回新增的ID</param>
        /// <returns></returns>
        public string Insert(T entity,bool returnId = true)
        {
            DbParameter[] parameters = null;

            NonQueryBuilder sqlBuilder = new NonQueryBuilder(this.DbHelper, DbOperation.Insert);
            sqlBuilder.SetFromTable(this.CurrentTableName);

            this._iEntity.SetEntity(sqlBuilder, entity); 
            string sql = sqlBuilder.BuildSQL(out parameters);

            return DoInsert(parameters, sql, returnId); 
        }


        public void Insert(List<T> entities)
        {
            // TODO 加个Insert List
        }

        /// <summary>
        /// 插入指定键值对的数据
        /// </summary>
        /// <param name="targetKVPs">需要插入的数据</param>
        /// <param name="returnId">返回新增的ID</param>
        /// <returns></returns>
        public string Insert(List<KeyValuePair<string, object>> targetKVPs, bool returnId = false)
        {
            DbParameter[] parameters = null;
              
            NonQueryBuilder sqlBuilder = new NonQueryBuilder(this.DbHelper, DbOperation.Insert);
            sqlBuilder.SetFromTable(this.CurrentTableName);

            foreach (KeyValuePair<string, object> kvp in targetKVPs)
            {
                sqlBuilder.SetValue(kvp.Key, kvp.Value);
            }
            string sql = sqlBuilder.BuildSQL(out parameters);

            return DoInsert(parameters, sql, returnId);
        }

        private string DoInsert(DbParameter[] parameters, string sql, bool returnId = false, bool identy = true)
        {
            string returnValue = string.Empty;
            #region 返回新增的主键值
            if (identy)
            {
                //自增
                switch (this.DbHelper.CurrentDbType)
                {
                    case DbTypeName.SqlServer:
                        // 需要返回主键
                        if (returnId)
                        {
                            sql += "; SELECT SCOPE_IDENTITY(); ";
                        }
                        break;
                    case DbTypeName.Access:
                        // 需要返回主键
                        if (returnId)
                        {
                            string returnIdSql = " SELECT @@identity AS ID FROM " + this.CurrentTableName + "; ";
                            returnValue = this.DbHelper.ExecuteScalar(returnIdSql).ToString();
                        }
                        break;
                }
            }
            #endregion

            if (identy && this.DbHelper.CurrentDbType == DbTypeName.SqlServer)
            {
                // 读取返回值
                if (returnId)
                {
                    returnValue = this.DbHelper.ExecuteScalar(sql, parameters).ToString();
                }
                else
                {
                    // 执行语句
                    returnValue = this.DbHelper.ExecuteNonQuery(sql, parameters).ToString();
                }
            }
            else
            {
                // 执行语句
                returnValue = this.DbHelper.ExecuteNonQuery(sql, parameters).ToString();
            }
            return returnValue;
        }
    }
}
