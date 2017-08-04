using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JSNet.Utilities;
using JSNet.Model;
using JSNet.DbUtilities;

namespace UnitTest
{
    /// <summary>
    /// 这里的都用代码生成器生成
    /// </summary>
    public class UserEntity :BaseEntity, IEntity<UserEntity>
    {
        private int? id = null;
        public int? Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        private string f1 = null;
        public string F1
        {
            get
            {
                return this.f1;
            }
            set
            {
                this.f1 = value;
            }
        }

        private string f2 = null;
        public string F2
        {
            get
            {
                return this.f2;
            }
            set
            {
                this.f2 = value;
            }
        }

        private string f3 = null;
        public string F3
        {
            get
            {
                return this.f3;
            }
            set
            {
                this.f3 = value;
            }
        }

        private DateTime? dateTimeType = null;
        public DateTime? DateTimeType
        {
            get
            {
                return this.dateTimeType;
            }
            set
            {
                this.dateTimeType = value;
            }
        }

        private int? numberType = null;
        public int? NumberType
        {
            get
            {
                return this.numberType;
            }
            set
            {
                this.numberType = value;
            }
        }

        private double? doubleType = null;
        public double? DoubleType
        {
            get
            {
                return this.doubleType;
            }
            set
            {
                this.doubleType = value;
            }
        }

        private float? floatType = null;
        public float? FloatType
        {
            get
            {
                return this.floatType;
            }
            set
            {
                this.floatType = value;
            }
        }

        private decimal? decimalType = null;
        public decimal? DecimalType
        {
            get
            {
                return this.decimalType;
            }
            set
            {
                this.decimalType = value;
            }
        }

        private int? nullType = null;
        public int? NullType
        {
            get
            {
                return this.nullType;
            }
            set
            {
                this.nullType = value;
            }
        }

        public string FieldF1
        {
            get { return "F1"; }
        }

        public string FieldF2
        {
            get { return "F2"; }
        }

        public string FieldF3 
        {
            get { return "F3"; }
        }

        public string FieldDateTimeType
        {
            get { return "DateTimeType"; }
        }

        public string FieldNumberType
        {
            get { return "NumberType"; }
        }

        public string FieldDoubleType
        {
            get { return "DoubleType"; }
        }

        public string FieldFloatType
        {
            get { return "FloatType"; }
        }

        public string FieldDecimalType
        {
            get { return "DecimalType"; }
        }

        public string FieldNullType
        {
            get { return "NullType"; }
        }

        public string TableName
        {
            get { return "[User]"; }
        }

        public UserEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.Id = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.F1 = CommonUtil.ConvertToString(dataRow[this.FieldF1]);
            this.F2 = CommonUtil.ConvertToString(dataRow[this.FieldF2]);
            this.F3 = CommonUtil.ConvertToString(dataRow[this.FieldF3]);
            this.DateTimeType = CommonUtil.ConvertToDateTime(dataRow[this.FieldDateTimeType]);
            this.NumberType = CommonUtil.ConvertToInt(dataRow[this.FieldNumberType]);
            this.DoubleType = CommonUtil.ConvertToDouble(dataRow[this.FieldDoubleType]);
            this.FloatType = CommonUtil.ConvertToFloat(dataRow[this.FieldFloatType]);
            this.DecimalType = CommonUtil.ConvertToDecimal(dataRow[this.FieldDecimalType]);
            this.NullType = CommonUtil.ConvertToInt(dataRow[this.FieldNullType]);
            return this;
        }

        public UserEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.Id = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.F1 = CommonUtil.ConvertToString(dataReader[this.FieldF1]);
            this.F2 = CommonUtil.ConvertToString(dataReader[this.FieldF2]);
            this.F3 = CommonUtil.ConvertToString(dataReader[this.FieldF3]);
            this.DateTimeType = CommonUtil.ConvertToDateTime(dataReader[this.FieldDateTimeType]);
            this.NumberType = CommonUtil.ConvertToInt(dataReader[this.FieldNumberType]);
            this.DoubleType = CommonUtil.ConvertToDouble(dataReader[this.FieldDoubleType]);
            this.FloatType = CommonUtil.ConvertToFloat(dataReader[this.FieldFloatType]);
            this.DecimalType = CommonUtil.ConvertToDecimal(dataReader[this.FieldDecimalType]);
            this.NullType = CommonUtil.ConvertToInt(dataReader[this.FieldNullType]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, UserEntity entity)
        {
            sqlBuilder.SetValue(this.FieldF1, entity.F1);
            sqlBuilder.SetValue(this.FieldF2, entity.F2);
            sqlBuilder.SetValue(this.FieldF3, entity.F3);
            sqlBuilder.SetValue(this.FieldDateTimeType, entity.DateTimeType);
            sqlBuilder.SetValue(this.FieldNumberType, entity.NumberType);
            sqlBuilder.SetValue(this.FieldDoubleType, entity.DoubleType);
            sqlBuilder.SetValue(this.FieldFloatType, entity.FloatType);
            sqlBuilder.SetValue(this.FieldDecimalType, entity.DecimalType);
            sqlBuilder.SetValue(this.FieldNullType, entity.NullType);
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
