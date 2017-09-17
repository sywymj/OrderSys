using DemoSys.MiniUI.Models;
using DemoSys.Service;
using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoSys.MiniUI.Controllers
{
    public class DemoController : BaseController
    {
        //
        // GET: /Demo/
        private DemoService service = new DemoService();

        [HttpGet]
        public ActionResult DemoIndex()
        {
            return View("~/Areas/Admin/Views/Demo/Demo_Index.cshtml");
        }

        #region 获取【树形】数据，以【TreeDDL】形式展示
        [HttpGet]
        public string GetDemoTreeDDL()
        {
            List<DemoEntity> list = service.GetDemoTreeList("XXX", true);
            var re = list.Select(l => new ViewDemoTreeDDL()
            {
                ID = l.ID.ToString(),
                ParentID = l.ParentID.ToString(),
                Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        #region 获取【树形】数据，但以【普通DDL】形式展示
        [HttpGet]
        public string GetDemoCodeDDL()
        {
            List<DemoEntity> list = service.GetDemoTreeList("XXX", true);
            var re = list.Select(l => new ViewDemoCodeDDL()
            {
                ID = l.Code,
                Title = l.Code
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        #region 获取【全部】数据，以【普通DDL】形式展示
        [HttpGet]
        public string GetDemoDDL()
        {
            List<DemoEntity> list = service.GetDemoList();

            var re = list.Select(l =>
                new ViewDemoDDL()
                {
                    ID = l.ID.ToString(),
                    Title = l.FullName
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        #region 获取【枚举Enum】数据，以【普通DDL】形式展示
        [HttpGet]
        public string GetDemoDDL()
        {
            Dictionary<DemoType, string> dic = EnumExtensions.ConvertToEnumDic<DemoType>();
            var re = dic.Select(d => new ViewDemoDDL()
            {
                ID = d.Key.ToString(),
                Title = d.Value
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        // TODO 增删该茶
        #region 增

        [HttpGet]
        public ActionResult InsertDemoIndex()
        {
            return View("~/Areas/Admin/Views/Demo/Demo_InsertIndex.cshtml");
        }

        #region 【最简单】的新增
        [HttpPost]
        public string AddDemo()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewDemo viewModel = FastJSON.JSON.ToObject<ViewDemo>(s);

            //TODO 数据验证。

            DemoEntity entity = new DemoEntity();
            viewModel.CopyTo(entity);
            service.AddDemo(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }  
        #endregion

        #region 【有外键实体】的新增
        [HttpPost]
        public string AddDemo1()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewDemo viewModel = FastJSON.JSON.ToObject<ViewDemo>(s);
            string sSonIDs = JSRequest.GetRequestFormParm("SonIDs");
            int[] sonIDs = JSValidator.ValidateStrings("儿子ID", sSonIDs, true);

            //TODO 数据验证。

            DemoEntity entity = new DemoEntity();
            viewModel.CopyTo(entity);
            SonEntity son = new SonEntity();
            viewModel.Son.CopyTo(son);//将外键实体赋值到Entity
            service.AddDemo(entity, son, sonIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }
        #endregion

        #endregion

        #region 改

        [HttpGet]
        public ActionResult EditDemoIndex()
        {
            return View("~/Areas/Admin/Views/Demo/Demo_InsertIndex.cshtml");
        }

        #region 【最简单】的新增
        [HttpPost]
        public string EditDemo()
        {
            //获取参数
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewDemo viewModel = FastJSON.JSON.ToObject<ViewDemo>(s);

            //TODO 数据验证。

            //ViewModel赋值
            DemoEntity Demo = new DemoEntity();
            viewModel.CopyTo(Demo);

            //调用Service
            service.EditDemo(Demo);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }
        #endregion

        #region 【有外键实体】的新增
        [HttpPost]
        public string EditDemo1()
        {
            //获取参数
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewDemo viewModel = FastJSON.JSON.ToObject<ViewDemo>(s);
            string sSonIDs = JSRequest.GetRequestFormParm("SonIDs");
            int[] roleIDs = JSValidator.ValidateStrings("儿子ID", sSonIDs, true);

            //TODO 数据验证。

            //ViewModel赋值
            DemoEntity entity = new DemoEntity();
            viewModel.CopyTo(entity);
            SonEntity son = new SonEntity();
            viewModel.Son.CopyTo(son);
            
            //调用Service
            service.EditDemo(entity, son, roleIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }
        #endregion

        #endregion

        //查
        [HttpGet]
        public string GetSingleDemo(int demoID) 
        {
            ViewDemo viewModel = new ViewDemo();
            DemoEntity entity = service.GetDemo(demoID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        }

        [HttpGet]
        public string GetDemoList()
        {
            int count = 0;
            List<DemoEntity> re = service.GetDemoList(out count);

            string s = JSON.ToJSON(new JSResponse(new ListData<DemoEntity>(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetDemoListByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            List<DemoEntity> re = service.GetDemoList(paging, out count);

            string s = JSON.ToJSON(new JSResponse(new ListData<DemoEntity>(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetDemoDT()
        {
            int count = 0;
            DataTable re = service.GetDemoDT(out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetDemoDTByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = service.GetDemoDT(paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetDemoTreeList()
        {
            List<DemoEntity> re = service.GetDemoTreeList("XXX");

            string s = JSON.ToJSON(new JSResponse(new List<DemoEntity>(re)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetDemoTreeDT()
        {
            DataTable re = service.GetDemoTreeDT("XXX");

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re)), jsonParams);
            return s;
        }


        [HttpGet]
        public string VerifyDemoCode(string demoCode, string demoID)
        {
            bool re = false;

            if (service.ChkDemoCodeExist(demoCode, demoID))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
        }

    }
}
