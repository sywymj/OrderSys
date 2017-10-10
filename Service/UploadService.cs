using JSNet.BaseSys;
using JSNet.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace JSNet.Service
{
    public class UploadService
    {

        private static string _UploadPath = "/Upload/";
        private static string _AllowExt = "jpg,bmp,jpeg,gif,png,ico";
        private static int _LimitSize = 2 * 1024 * 1024;//2M

        public static string UploadPath
        {
            get { return _UploadPath; }
        }

        public void FileSaveAs(HttpPostedFileBase httpPostedFile, out string newFileName)
        {
            newFileName = CommonUtil.NewGuid() + "." + CommonUtil.GetFileExt(httpPostedFile.FileName);
            string localUploadPath = CommonUtil.GetMapPath(_UploadPath);
            string newFileFullName = localUploadPath + newFileName;

            //检查扩展名是否合法
            if (!CheckExt(CommonUtil.GetFileExt(newFileFullName)))
            {
                throw new JSException("扩展名不合法！");
            }
            if (!IsImage(CommonUtil.GetFileExt(newFileFullName)))
            {
                throw new JSException("扩展名不合法！");
            }
            //检查文件大小是否合法
            if (httpPostedFile.ContentLength > _LimitSize)
            {
                throw new JSException(string.Format("文件不能大于{0}M！", _LimitSize / 1024));
            }
            //检查上传的临时文件目录物理路径是否存在，不存在则创建
            if (!Directory.Exists(localUploadPath))
            {
                Directory.CreateDirectory(localUploadPath);
            }

            //保存文件
            httpPostedFile.SaveAs(newFileFullName);
        }

        public void RemoveFile(string fileName)
        {
            string fileFullName = _UploadPath + fileName;

            bool deleteFileSuccessed = CommonUtil.DeleteFile(fileFullName);
            if (!deleteFileSuccessed)
            {
                throw new JSException("删除失败，请重试！");
            }
        }

        private bool CheckExt(string strFileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "ashx", "asa", "asmx", "asax", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == strFileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件（程序允许的文件后缀）
            string[] allowExt = _AllowExt.Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == strFileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            al.Add("ico");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
