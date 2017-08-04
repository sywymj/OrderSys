using System;
using System.Collections.Generic;
using System.Data;

namespace JSNet.Model
{

    [Serializable]
    public abstract class BaseEntity
    {
        /// <summary>
        /// 主键 字段
        /// </summary>
        public virtual string PrimaryKey 
        {
            get { return "ID"; }
        }

        /// <summary>
        /// 主键自动递增
        /// </summary>
        public virtual bool IsIdenty
        {
            get { return false; }
        }

        /// <summary>
        /// 删除标志 字段
        /// </summary>
        public virtual string DeletionStateCode
        {
            get { return null; }
        }
    }
}