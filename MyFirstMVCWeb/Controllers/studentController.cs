using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstMVCWeb.Models;


namespace MyFirstMVCWeb.Controllers
{
    public class studentController : Controller
    {
        userEntities20 db = new userEntities20();

        
        public ActionResult home()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            return View();
        }

        public ActionResult student()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            Book s = new Book();
            List<Book> z = new List<Book>();
            List<rollcallTable_1> z1 = db.rollcallTable_1.ToList();
            string status = Convert.ToString(Session["UserID"]); ;//登入者的學號
            List<course1Table_1> we = db.course1Table_1.ToList();
            ViewBag.data2 = we;
            List<classromTable_1> wz = db.classromTable_1.ToList();
            ViewBag.data4 = wz;
            List<string> a = new List<string>();
            List<string> b = new List<string>();//學生選課代號
            List<allrollcallTable_1> wr = db.allrollcallTable_1.ToList();
            string i = "";
            int ps = 0;
            int zw = 0;
            ///選課名稱
            foreach (var q in wz)
            {
                int qw = 0;
                ps++;
                foreach (var zq in wr)
                {
                    if (zq.status == status)
                    {
                        if (zq.attend == "未到課")
                        {
                            if (zq.class2 == q.Course)
                            {
                                qw++;
                            }
                        }
                    }
                }
                if (qw > 0)
                {
                    classromTable_1 wk = db.classromTable_1.FirstOrDefault(t => t.Course == q.Course);
                    if (wk != null)
                    {
                        string qw1 = qw.ToString();
                        b.Add(wk.classroom);
                        b.Add(qw1);
                    }

                }
            }
            ////抓取點名總紀錄裡的日期
            foreach (var wq in wr)
            {
                if (wq.data != i)
                {
                    string w = a.FirstOrDefault(t => t == wq.data);
                    if (w == null)
                    {
                        i = wq.data;
                        a.Add(wq.data);
                    }

                }
            }
            ViewBag.attend = b;//曠課數量
            ViewBag.data5 = a;//日期
            List<rollcallTable_1> j = db.rollcallTable_1.ToList();
            ViewBag.data6 = j;
            string Date = DateTime.Now.ToString();

            Date = System.Globalization.DateTimeFormatInfo.CurrentInfo.DayNames[(byte)(DateTime.Now.DayOfWeek)];
            return View();
        }
        [HttpPost]
        public ActionResult student1(string select1, string select2)
        {
            if (select2 == "選擇出席狀態")
            {
                return RedirectToAction("student");
            }

            List<course1Table_1> we = db.course1Table_1.ToList();
            ViewBag.data2 = we;
            List<classromTable_1> wz = db.classromTable_1.ToList();
            ViewBag.data4 = wz;
            classromTable_1 zm = db.classromTable_1.FirstOrDefault(t => t.Course == select1);
            if (zm != null)
            {
                ViewBag.class1 = zm.classroom;
            }
            ////抓取點名總紀錄裡的日期
            List<rollcallTable_1> z = db.rollcallTable_1.ToList();
            List<allrollcallTable_1> w = db.allrollcallTable_1.ToList();
            ViewBag.all = w;
            ViewBag.data6 = z;
            ViewBag.se = select1;
            ViewBag.se1 = select2;
            return View("student");
        }

        public ActionResult register()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            string status = Convert.ToString(Session["UserID"]); //登入者的學號
            string name = Convert.ToString(Session["name"]);
            student1Table_1 we = db.student1Table_1.FirstOrDefault(t => t.stud == status);
            ViewBag.naME = we.name;
            List<student1Table_1> st = db.student1Table_1.ToList();
            List<string> class1 = new List<string>();
            foreach (var e in st)
            {
                if (e.class1 != we.class1)
                {
                    string z = class1.FirstOrDefault(t => t == e.class1);
                    if (z == null)
                    {
                        class1.Add(e.class1);
                    }
                }
            }
            if (we != null)
            {
                ViewBag.class2 = we.class1;
                ViewBag.phone = we.phone;
            }
            ViewBag.class1 = class1;
            return View();
        }
        [HttpPost]
        public ActionResult change(string name, string select1, string phone, string pass)
        {
            string status = Convert.ToString(Session["UserID"]); //登入者的學號
            List<student1Table_1> p = db.student1Table_1.ToList();
            student1Table_1 we = db.student1Table_1.FirstOrDefault(t => t.stud == status);
            string sw = "";
            if (we != null)
            {
                if (name != "" && name != null)
                {
                    we.name = name;

                }
                if (select1 != null && select1 != "")
                {
                    we.class1 = select1;

                }
                if (phone != "" && phone != null)
                {
                    if (phone.Length != 0)
                    {
                        foreach (var z in p)
                        {
                            if (z.stud != status)
                            {
                                if (z.phone == phone)
                                {
                                    TempData["messg1"] = "你的電話號碼與人重複";
                                    sw = "1";
                                }
                            }


                        }
                        if (sw != "1")
                        {
                            we.phone = phone;

                        }
                    }

                }

                db.SaveChanges();
            }
            firstTable_2 e = db.firstTable_2.FirstOrDefault(t => t.user == status);
            if (e != null)
            {
                if (name != "" && name != null)
                {
                    e.name = name;
                }
                if (pass != null && pass != "")
                {
                    firstTable_2 x = db.firstTable_2.FirstOrDefault(t => t.password == pass);
                    if (x == null)
                    {
                        e.password = pass;
                        TempData["messg3"] = "更改成功";
                    }
                    else
                    {
                        if (x.user == e.user)
                        {
                            TempData["messg2"] = "請輸入不一樣的密碼";
                        }
                        else
                        {
                            TempData["messg3"] = "更改成功";
                        }
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("register", "student");
        }
    }
}