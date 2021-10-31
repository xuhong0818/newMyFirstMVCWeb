using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstMVCWeb.Models;

namespace MyFirstMVCWeb.Controllers
{
    public class hphController : Controller
    {
        // GET: hph
        //first2Entities3 dc = new first2Entities3();
        public ActionResult Index()
        {
            //為下拉列表創造一個選項的集合
           ViewData["cd"] = new List<SelectListItem>(){
            new SelectListItem() { Selected = true, Text = "增加", Value = "1" },
            new SelectListItem() { Selected = true, Text = "刪除", Value = "2" },
            new SelectListItem() { Selected = true, Text = "編輯", Value = "3" },
            } ;
            return View();
        }

        public ActionResult data1()
        {
           // ViewData.Model = dc.firstTable_2.ToList();
            return View();
    
        }

        public ActionResult test()
        {
            //////創立對象qw
            Class1 qw = new Class1()
            {
                id = 1,
                name = "高續",
                age = 20
            };
            ///強類型使用方式
            ///方式1:將對象傳給ViewData.Model屬性就可以實現強類型
            ///方式2:view(訪問視圖名稱,對象)
            ////ViewData.Model = qw;
            return View(qw);
        }
    }

}