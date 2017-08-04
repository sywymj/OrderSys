using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JSNet.Utilities;
using JSNet.Model;
using JSNet.DbUtilities;
namespace JSNet.Model
{
    //S_TabSN
    public class TabSNEntity : BaseEntity, IEntity<TabSNEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[S_TabSN]"; }
        }





        #region 字段、属性
        /// <summary>
        /// Sn
        /// </summary>		
        private int? _sn;
        public int? Sn
        {
            get { return _sn; }
            set { _sn = value; }
        }
        /// <summary>
        /// SnDt
        /// </summary>		
        private DateTime? _sndt;
        public DateTime? SnDt
        {
            get { return _sndt; }
            set { _sndt = value; }
        }
        #endregion

        #region  数据库字段名称
        /// <summary>
        /// FiledName:Sn
        /// </summary>		
        public static string FieldSn
        {
            get { return "Sn"; }
        }
        /// <summary>
        /// FiledName:SnDt
        /// </summary>		
        public static string FieldSnDt
        {
            get { return "SnDt"; }
        }
        #endregion

        public TabSNEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.Sn = CommonUtil.ConvertToInt(dataRow[FieldSn]);
            this.SnDt = CommonUtil.ConvertToDateTime(dataRow[FieldSnDt]);
            return this;
        }


        public TabSNEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.Sn = CommonUtil.ConvertToInt(dataReader[FieldSn]);
            this.SnDt = CommonUtil.ConvertToDateTime(dataReader[FieldSnDt]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, TabSNEntity entity)
        {
            sqlBuilder.SetValue(FieldSn, entity.Sn);
            sqlBuilder.SetValue(FieldSnDt, entity.SnDt);
        }

        public void GetFromExpand(System.Data.DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void GetFromExpand(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}