using JSNet.DbUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace JSNet.Model
{
    public interface IEntity<T>
        where T : BaseEntity
    {
        string TableName { get; }

        string PrimaryKey { get; }

        string DeletionStateCode { get; }

        bool IsIdenty { get; } 

        void GetFromExpand(DataRow dataRow);

        void GetFromExpand(IDataReader dataReader);

        T GetFrom(DataRow dataRow);

        T GetFrom(IDataReader dataReader);

        void SetEntity(NonQueryBuilder sqlBuilder, T entity);

    }
}
