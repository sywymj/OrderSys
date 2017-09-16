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

        [HttpGet]
        public ActionResult InsertDemoIndex()
        {
            return View("~/Areas/Admin/Views/Demo/Demo_InsertIndex.cshtml");
        }

        [HttpGet]
        public ActionResult EditDemoIndex()
        {
            return View("~/Areas/Admin/Views/Demo/Demo_InsertIndex.cshtml");
        }

        //获取【树形】数据，以【TreeDDL】形式展示
        [HttpGet]
        public string GetDemoTreeDDL()
        {
            List<DemoEntity> list = service.GetTreeDemoList("XXX", true);
            var re = list.Select(l => new ViewDemoTreeDDL()
            {
                ID = l.ID.ToString(),
                ParentID = l.ParentID.ToString(),
                Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        //获取【树形】数据，但以【普通DDL】形式展示
        [HttpGet]
        public string GetDemoCodeDDL()
        {
            List<DemoEntity> list = service.GetTreeDemoList("XXX", true);
            var re = list.Select(l => new ViewDemoCodeDDL()
            {
                ID = l.Code,
                Title = l.Code
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        //获取【全部】数据，以【普通DDL】形式展示
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

        //获取【枚举Enum】数据，以【普通DDL】形式展示
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

        // TODO 增删该茶
    }
}
