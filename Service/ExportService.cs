using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace JSNet.Service
{
    public class ExportService
    {
        private string _exportBasePath = "/Export/";
        public string GetExportFolderWebPath(string type,out string localpath)
        {
            string path = _exportBasePath;
            switch (type.ToLower())
            {
                case "order":
                    path += type.ToUpper();
                    break;
            }
            path += "/";
            //文件目录物理路径是否存在，不存在则创建
            localpath = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(localpath))
            {
                Directory.CreateDirectory(localpath);
            }

            return path;
        }

        /// <summary>
        /// 获取文件名，不带后缀
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            Random ran=new Random();
            int r=ran.Next(1,999);
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + r.ToString();
        }

    }
}
