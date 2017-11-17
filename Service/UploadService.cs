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
        public delegate void ValidateDelegate(HttpPostedFileBase httpPostedFile);  
        public static string UploadPath
        {
            get { return _UploadPath; }
        }


        public void FileSaveAsImage(HttpPostedFileBase httpPostedFile, out string fileFullName)
        {
            string fileName = "";
            FileSaveAs(httpPostedFile, _UploadPath + "OrderImg/", ValidateImage, out fileName);
            fileFullName = _UploadPath + "OrderImg/" + fileName;
        }

        public void FileSaveAsExcel(HttpPostedFileBase httpPostedFile, out string localFullName)
        {
            string fileName = "";
            FileSaveAs(httpPostedFile, _UploadPath + "UserImport/", ValidateExcel, out fileName);
            localFullName = CommonUtil.GetMapPath(_UploadPath + "UserImport/" + fileName);
        }

        public void FileSaveAs(HttpPostedFileBase httpPostedFile,string uploadPath,ValidateDelegate validate, out string newFileName)
        {
            newFileName = CommonUtil.NewGuid() + "." + CommonUtil.GetFileExt(httpPostedFile.FileName);
            string localUploadPath = CommonUtil.GetMapPath(uploadPath);
            string newFileFullName = localUploadPath + newFileName;

            validate(httpPostedFile);

            //检查上传的临时文件目录物理路径是否存在，不存在则创建
            if (!Directory.Exists(localUploadPath))
            {
                Directory.CreateDirectory(localUploadPath);
            }
            //保存文件
            httpPostedFile.SaveAs(newFileFullName);
        }

        public void RemoveFile(string fileFullName)
        {
            bool deleteFileSuccessed = CommonUtil.DeleteFile(fileFullName);
            if (!deleteFileSuccessed)
            {
                throw new JSException("删除失败，请重试！");
            }
        }

        private bool CheckDangerExt(string strFileExt)
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
            return true;
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

        private void ValidateImage(HttpPostedFileBase httpPostedFile)
        {
            int filesize = 2 * 1024 * 1024;


            if (!CheckDangerExt(CommonUtil.GetFileExt(httpPostedFile.FileName)))
            {
                throw new JSException("扩展名不合法！");
            }
            if (!IsImage(CommonUtil.GetFileExt(httpPostedFile.FileName)))
            {
                throw new JSException("只允许上传图片！");
            }
            if (httpPostedFile.ContentLength > filesize)
            {
                throw new JSException(string.Format("文件不能大于{0}M！", filesize / 1024));
            }
        }

        private void ValidateExcel(HttpPostedFileBase httpPostedFile)
        {
            int filesize = 10 * 1024 * 1024;
            string[] allowExt = { "xls", "xlsx" };

            if (!CheckDangerExt(CommonUtil.GetFileExt(httpPostedFile.FileName)))
            {
                throw new JSException("扩展名不合法！");
            }
            if (!allowExt.Contains(CommonUtil.GetFileExt(httpPostedFile.FileName)))
            {
                throw new JSException("只允许上传Excel文件！");
            }
            if (httpPostedFile.ContentLength > filesize)
            {
                throw new JSException(string.Format("文件不能大于{0}M！", filesize / 1024));
            }
        } 
    }
}
