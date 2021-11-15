using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstMVCWeb.Models;

namespace MyFirstMVCWeb.Controllers
{    //HomeController理解為 控制器名稱叫home, 
    //控制器後面必須加上單詞Controller,這是一個規定
    public class oneController : Controller
    {
        // GET: one
        userEntities20 db = new userEntities20();

        public ActionResult LogOff()
        {//登出

            Session.Clear();
            return RedirectToAction("register");
        }

        //獲取用戶登入資料
        [HttpPost]
        public ActionResult login(string stud, string Pwd)
        {
            //根據用戶輸入的數據查詢,如果不符合提醒用戶並重新輸入
            //如果ok下一
            string time = Request.QueryString["cratetime"];
            string status = Request.QueryString["status"];
            firstTable_2 u = db.firstTable_2.FirstOrDefault(t => t.user == stud);
            if (u != null)
            {
                if (u.password == Pwd)
                {
                    if (u.power == "teacher")
                    {
                        Session["UserID"] = u.user;
                        Session["name"] = u.name;

                        return RedirectToAction("home", "data");
                    }
                    else
                    {
                        Session["UserID"] = u.user;
                        Session["name"] = u.name;
                        return RedirectToAction("student", "student");
                    }

                }
                else
                {
                    ViewBag.Error = "帳密有誤,請重新輸入";
                    return View("register");
                }
            }
            else
            {
                ViewBag.Error = "不存在的帳密";
                return View("register");
            }
        }
        public ActionResult register()
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            List<allrollcallTable_1> ew = db.allrollcallTable_1.ToList();
            return View();
        }
    }
}