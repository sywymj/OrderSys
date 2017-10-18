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
    //S_ApiLog
    public class ApiLogEntity : BaseEntity, IEntity<ApiLogEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[S_ApiLog]"; }
        }

        /// <summary>
        /// 主键
        /// </summary>
        public override string PrimaryKey
        {
            get { return "ID"; }
        }


        /// <summary>
        /// 主键自动递增
        /// </summary>
        public override bool IsIdenty
        {
            get { return true; }
        }


        #region 字段、属性
        /// <summary>
        /// ID
        /// </summary>		
        private int? _id;
        public int? ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// ApiType
        /// </summary>		
        private string _apitype;
        public string ApiType
        {
            get { return _apitype; }
            set { _apitype = value; }
        }
        /// <summary>
        /// DateTime
        /// </summary>		
        private DateTime? _datetime;
        public DateTime? DateTime
        {
            get { return _datetime; }
            set { _datetime = value; }
        }
        /// <summary>
        /// RequestUrl
        /// </summary>		
        private string _requesturl;
        public string RequestUrl
        {
            get { return _requesturl; }
            set { _requesturl = value; }
        }
        /// <summary>
        /// Response
        /// </summary>		
        private string _response;
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }
        /// <summary>
        /// Message
        /// </summary>		
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        /// <summary>
        /// ErrorCode
        /// </summary>		
        private string _errorcode;
        public string ErrorCode
        {
            get { return _errorcode; }
            set { _errorcode = value; }
        }
        /// <summary>
        /// ErrorMsg
        /// </summary>		
        private string _errormsg;
        public string ErrorMsg
        {
            get { return _errormsg; }
            set { _errormsg = value; }
        }
        #endregion

        #region  数据库字段名称
        /// <summary>
        /// FiledName:ID
        /// </summary>		
        public static string FieldID
        {
            get { return "ID"; }
        }
        /// <summary>
        /// FiledName:ApiType
        /// </summary>		
        public static string FieldApiType
        {
            get { return "ApiType"; }
        }
        /// <summary>
        /// FiledName:DateTime
        /// </summary>		
        public static string FieldDateTime
        {
            get { return "DateTime"; }
        }
        /// <summary>
        /// FiledName:RequestUrl
        /// </summary>		
        public static string FieldRequestUrl
        {
            get { return "RequestUrl"; }
        }
        /// <summary>
        /// FiledName:Response
        /// </summary>		
        public static string FieldResponse
        {
            get { return "Response"; }
        }
        /// <summary>
        /// FiledName:Message
        /// </summary>		
        public static string FieldMessage
        {
            get { return "Message"; }
        }
        /// <summary>
        /// FiledName:ErrorCode
        /// </summary>		
        public static string FieldErrorCode
        {
            get { return "ErrorCode"; }
        }
        /// <summary>
        /// FiledName:ErrorMsg
        /// </summary>		
        public static string FieldErrorMsg
        {
            get { return "ErrorMsg"; }
        }
        #endregion

        public ApiLogEntity GetFrom(System.Data.DataRow dataRow)
        {
            ApiLogEntity entity = new ApiLogEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.ApiType = CommonUtil.ConvertToString(dataRow[FieldApiType]);
            entity.DateTime = CommonUtil.ConvertToDateTime(dataRow[FieldDateTime]);
            entity.RequestUrl = CommonUtil.ConvertToString(dataRow[FieldRequestUrl]);
            entity.Response = CommonUtil.ConvertToString(dataRow[FieldResponse]);
            entity.Message = CommonUtil.ConvertToString(dataRow[FieldMessage]);
            entity.ErrorCode = CommonUtil.ConvertToString(dataRow[FieldErrorCode]);
            entity.ErrorMsg = CommonUtil.ConvertToString(dataRow[FieldErrorMsg]);
            return entity;
        }


        public ApiLogEntity GetFrom(System.Data.IDataReader dataReader)
        {
            ApiLogEntity entity = new ApiLogEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.ApiType = CommonUtil.ConvertToString(dataReader[FieldApiType]);
            entity.DateTime = CommonUtil.ConvertToDateTime(dataReader[FieldDateTime]);
            entity.RequestUrl = CommonUtil.ConvertToString(dataReader[FieldRequestUrl]);
            entity.Response = CommonUtil.ConvertToString(dataReader[FieldResponse]);
            entity.Message = CommonUtil.ConvertToString(dataReader[FieldMessage]);
            entity.ErrorCode = CommonUtil.ConvertToString(dataReader[FieldErrorCode]);
            entity.ErrorMsg = CommonUtil.ConvertToString(dataReader[FieldErrorMsg]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, ApiLogEntity entity)
        {
            sqlBuilder.SetValue(FieldApiType, entity.ApiType);
            sqlBuilder.SetValue(FieldDateTime, entity.DateTime);
            sqlBuilder.SetValue(FieldRequestUrl, entity.RequestUrl);
            sqlBuilder.SetValue(FieldResponse, entity.Response);
            sqlBuilder.SetValue(FieldMessage, entity.Message);
            sqlBuilder.SetValue(FieldErrorCode, entity.ErrorCode);
            sqlBuilder.SetValue(FieldErrorMsg, entity.ErrorMsg);
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