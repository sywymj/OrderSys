using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace JSNet.Utilities
{
    public class HttpHelper
    {
        public static WebUtils util = new WebUtils();


        /// <summary>  
        /// GET请求与获取结果  
        /// </summary>  
        public static string HttpGet(string Url, string postDataStr)
        {
          
            Url = Url + (postDataStr == "" ? "" : "?") + postDataStr;
            return util.DoGet(Url,null);

            #region 旧版本Get
            /*
          HttpWebRequest request = null;
          HttpWebResponse response = null;
          try
          {
              request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
              request.Method = "GET";
              request.ContentType = "text/html;charset=UTF-8";
              response = (HttpWebResponse)request.GetResponse();

              Stream myResponseStream = response.GetResponseStream();
              StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
              string retString = myStreamReader.ReadToEnd();

              myStreamReader.Close();
              myResponseStream.Close();
              return retString;

          }
          catch (Exception ex)
          {
              System.GC.Collect();
              LogHelper.LogFile("Http Get Error: " + ex.Message);
              return string.Empty;
          }
          finally
          {
              CollectResources(request, response);
          }
          */

            #endregion
        }

        /// <summary>  
        /// POST请求与获取结果  
        /// </summary>  
        public static string HttpPost(string Url, string postDataStr)
      {
          return util.DoPost(Url, Encoding.UTF8.GetBytes(postDataStr), "application/x-www-form-urlencoded", null);
          #region 旧版本Post
          /*
          HttpWebRequest request = null;
          HttpWebResponse response = null;
          try
          {
              byte[] postByte = Encoding.UTF8.GetBytes(postDataStr);
              System.GC.Collect();
              request = (HttpWebRequest)WebRequest.Create(Url);
              request.Method = "POST";
              request.ContentType = "application/x-www-form-urlencoded";

              //request.ContentLength = postDataStr.Length;
              //StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);

              request.ContentLength = postByte.Length;
              StreamWriter writer = new StreamWriter(request.GetRequestStream());

              writer.Write(postDataStr);
              writer.Flush();

              response = (HttpWebResponse)request.GetResponse();
              string encoding = response.ContentEncoding;
              if (encoding == null || encoding.Length < 1)
              {
                  encoding = "UTF-8"; //默认编码  
              }
              StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
              string retString = reader.ReadToEnd();
              return retString;
          }
          catch (Exception ex)
          {
              System.GC.Collect();
              LogHelper.LogFile("Http Post Error: " + ex.Message);
              return string.Empty;
          }
          finally
          {
              CollectResources(request, response);
          }
          */
            #endregion
        }

        #region Demo
        #region Get DEMO
        //string url = "http://www.mystudy.cn/LoginHandler.aspx";
        //string data = "UserName=admin&Password=123";
        //string result = HttpGet(url, data);  
        #endregion

        #region Post DEMO
        //string url = "http://www.mystudy.cn/LoginHandler.aspx";
        //string data = "UserName=admin&Password=123";
        //string result = HttpPost(url, data);
        #endregion DEMO
        #endregion

        /// <summary>
        /// 释放连接资源
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void CollectResources(HttpWebRequest request, HttpWebResponse response)
        {
            if (null != request)
            {
                request.Abort();
            }
            if (null != response)
            {
                response.Close();
            }
        }
    }
}
