using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFirstMVCWeb.Models;
using System.Net.Mail;
/// <summary>
/// 現在作法:
/// 1.先把當天上課學員存在暫時點名
/// 2.再讓前端去判斷哪個課程顯示哪個課的同學
/// 3.按鈕都是用post
/// 4.課選單是用下拉選單具體導入頁面方法還在想?
/// 5.收尋方法
/// 
/// 應該要作法2:
/// 1.把當天全部的學員都放在 暫時點名表 
/// 2.再去個別顯示每堂課的學員名單,這樣老師可以提前查看
/// </summary>
//1.加入提醒框和補點特別標示(解決)
//2.未到課做一個最後確認(解決)
//3.搜索頁面
//3.1刷新點名頁面(解決)
//4.點名紀錄表加日期分別不同課的紀錄(解決)
//5.補點名問題解決
//6.把國文頁面做完
//7.按鈕除了提交補點外都有問題(解決)
///按鈕立體

///一學期18週
///設定第18週最後一堂課的日期以此判斷一學期

namespace MyFirstMVCWeb.Controllers
{
    public class DataController : Controller
    {
        userEntities20 db = new userEntities20();

        public ActionResult Home()//補點名(解決) 按完補點名回到頁面
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            return View();
        }

        public ActionResult studentmenu(string b)//補點名(解決) 按完補點名回到頁面
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            ViewBag.course = b;
            string teacher_name = Convert.ToString(Session["name"]);
            teacher1 te = db.teacher1.FirstOrDefault(t => t.teachername == teacher_name);
            if (te != null)
            {
                ViewBag.teacherid = te.teacher;
            }
            List<classromTable_1> cla = db.classromTable_1.ToList();
            ViewBag.cla = cla;
            List<student1Table_1> st = db.student1Table_1.ToList();
            ViewBag.stu = st;
            List<course1Table_1> coy = db.course1Table_1.ToList();
            ViewBag.coy = coy;
            List<string> class1 = new List<string>();

            foreach (var e in st)
            {
                string z = class1.FirstOrDefault(t => t == e.class1);
                if (z == null)
                {
                    class1.Add(e.class1);
                }
            }
            ViewBag.class1 = class1;
            return View();

        }

        //save儲存表單傳回來的數值
        //從前端視圖獲取數據
        //save(FormCollection sa) 提交給save
        //把表單裡的數據存在FormCollection和sa裡 就能從兩者找值
        ///post過來的數值儲存在string陣列裡再用foreach迭代出來
        [HttpPost]
        public ActionResult Edit(string[] status, string class2, string select2, string name32)//補點名(解決) 按完補點名回到頁面
        {
            var w1 = Request.UrlReferrer.Segments[2];
            List<rollcallTable_1> bn = db.rollcallTable_1.ToList();
            if (status != null)
            {
                foreach (var status1 in status)
                {
                    foreach (var bc in bn)
                    {
                        if (bc.class2 == class2)
                        {
                            if (bc.status == status1)
                            {
                                String time = DateTime.Now.ToString("HH:mm:ss");
                                bc.time = time;
                                bc.attend = "補點";
                                db.SaveChanges();
                            }

                        }
                    }
                }
            }

            List<allrollcallTable_1> all = db.allrollcallTable_1.ToList();
            if (status != null)
            {
                foreach (var status1 in status)
                {
                    foreach (var al in all)
                    {
                        if (al.status == status1)
                        {
                            if (al.class2 == class2)
                            {
                                if (al.name == name32)
                                    if (al.data == select2)
                                    {

                                        al.time = DateTime.Now.ToString("yyyy-MM-dd") + DateTime.Now.ToString("HH:mm:ss");//日期
                                        al.attend = "補點";
                                        db.SaveChanges();
                                    }
                            }
                        }
                    }
                }
            }

            if (w1 == "one2")
            {
                return RedirectToAction("one3", new { class2, select2 });
            }
            else
            {
                return RedirectToAction("exit1", new { class2 });
            }
        }
        public ActionResult one3(string select1, string select2, string class2)
        {
            List<classromTable_1> wx = db.classromTable_1.ToList();
            ViewBag.data4 = wx;
            List<string> a = new List<string>();
            List<allrollcallTable_1> wr = db.allrollcallTable_1.ToList();
            List<allrollcallTable_1> wb = db.allrollcallTable_1.ToList();
            classromTable_1 zw = db.classromTable_1.FirstOrDefault(t => t.Course == select1);
            if (zw != null)
            {
                ViewBag.classroom = zw.classroom;
            }
            string te = Convert.ToString(Session["name"]);
            teacher1 t22 = db.teacher1.FirstOrDefault(t2 => t2.teachername == te);
            if (t22 != null)
            {
                ViewBag.teh = t22.teacher;
            }
            ViewBag.data3 = wr;
            ViewBag.select1 = class2;
            ViewBag.select2 = select2;
            string i = "";
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
            ViewBag.data1 = a;
            //日期
            return View("one1");
        }

        //點名紀錄儲存(解決)
        public ActionResult Sav2(string class2)
        {///1.第一次加入全部資料 2.儲存補點資料 (解決)
            List<allrollcallTable_1> rw = db.allrollcallTable_1.ToList();
            List<rollcallTable_1> ro = db.rollcallTable_1.ToList();
            allrollcallTable_1 all = new allrollcallTable_1();
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            allrollcallTable_1 all1 = db.allrollcallTable_1.FirstOrDefault(t => t.data == Date);
            if (all1 == null)
            { ///做完第一次加入
                foreach (var a in ro)
                {
                    all.attend = a.attend;
                    all.status = a.status;
                    all.class1 = a.class1;
                    all.class2 = a.class2;
                    all.name = a.name;
                    all.data = a.data;
                    all.time = a.time;
                    db.allrollcallTable_1.Add(all);
                    db.SaveChanges();
                }
            }
            ////儲存更新資料
            foreach (var a in ro)//暫時點名表
            {
                foreach (var se in rw)//歷史點名表
                {
                    if (a.status == se.status)
                    {
                        if (a.class2 == se.class2)
                        {
                            if (a.data == se.data)
                            {
                                se.time = a.time;
                                se.attend = a.attend;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("exit1", new { class2 });
        }
        [HttpPost]
        //點名紀錄
        //更改點名紀錄(解決)
        public ActionResult del(string[] status, string class2)
        {///最後一次保存所有資料
            List<rollcallTable_1> bn = db.rollcallTable_1.ToList();
            if (status != null)
            {
                foreach (var status1 in status)
                {
                    foreach (var bc in bn)
                    {
                        if (bc.class2 == class2)
                        {
                            if (bc.status == status1)
                            {
                                if (bc.attend == "補點")
                                {
                                    bc.attend = "未到課";
                                    bc.time = null;
                                    db.SaveChanges();
                                }
                            }

                        }
                    }
                }
            }
            return RedirectToAction("exit1", new { class2 });
        }

        public ActionResult exit2()
        {
            string time = Request.QueryString["cratetime"];
            string status = Request.QueryString["status"];
            ////人臉辨識 先去判斷時間 判斷完時間再去判斷課程 之後去找學生資料更新
            List<rollcallTable_1> Q = db.rollcallTable_1.ToList();
            List<classromTable_1> q = db.classromTable_1.ToList();
            foreach (var z in q)
            {
                //if (z.week == tmp)
                //{
                //    double w = Convert.ToDouble(z.time);
                //    w = w + 5;
                //    oe = w - 65;
                //    if (nowtim > oe & nowtim < w)
                //    {
                foreach (var n in Q)
                {
                    if (z.Course == n.class2)
                    {
                        if (n.status == status)
                        {
                            student1Table_1 st = db.student1Table_1.FirstOrDefault(t => t.stud == status);
                            if (st != null)
                            {

                                MailMessage mail = new MailMessage();
                                //前面是發信email後面是顯示的名稱
                                mail.From = new MailAddress("s98411158@gmail.com", status + "此封為點名成功Reminder信件");
                                //收信者email
                                mail.To.Add(st.allemail);
                                //設定優先權
                                mail.Priority = MailPriority.Normal;
                                //標題
                                mail.Subject = "test";
                                //內容
                                string s = "此堂為" + z.classroom;
                                mail.Body = s + "<p>你已於</p>" + time + "<h1>成功辨識</h1>";
                                //內容使用html
                                mail.IsBodyHtml = true;
                                //設定gmail的smtp (這是google的)
                                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                                //您在gmail的帳號密碼
                                MySmtp.Credentials = new System.Net.NetworkCredential("s98411158@gmail.com", "8610085x");
                                //開啟ssl
                                MySmtp.EnableSsl = true;
                                //發送郵件
                                MySmtp.Send(mail);
                                //放掉宣告出來的MySmtp
                                MySmtp = null;
                                //放掉宣告出來的mail
                                mail.Dispose();
                            }


                            n.time = time;
                            n.attend = "已到課";
                            db.SaveChanges();
                        }
                    }
                }
                //    }
                //}
            }
            return RedirectToAction("exit1", new { time, status });
        }

        public ActionResult exit1(string a, string b, string class2, string time, string status)
        {//國文課14點上課 英文課9.上課  數學課17點上課

            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            string te = Convert.ToString(Session["name"]);
            teacher1 t22 = db.teacher1.FirstOrDefault(t2 => t2.teachername == te);
            if (t22 != null)
            {
                ViewBag.teh = t22.teacher;
            }
            List<classromTable_1> wx = db.classromTable_1.ToList();
            ViewBag.data4 = wx;
            ///第一次加入暫時時點名表
            string course2 = class2;//回到方法頁面
            //每天加入新的點名紀錄
            string Date = DateTime.Now.ToString("yyyy-MM-dd");//日期
            string tmp = DateTime.Now.DayOfWeek.ToString();
            rollcallTable_1 w = new rollcallTable_1();
            ///判斷哪些人有修這堂課,把有修的抓出來
            rollcallTable_1 bx = db.rollcallTable_1.FirstOrDefault(t => t.data == Date);
            if (bx == null)
            {
                foreach (var ns in wx)
                {
                    if (ns.week == tmp)
                    {
                        List<course1Table_1> co1 = db.course1Table_1.ToList();
                        foreach (var ce in co1)
                        {//把每個學生照著選課加進點名單
                            if (ce.course == ns.Course)
                            {
                                student1Table_1 c = db.student1Table_1.FirstOrDefault(t => t.stud == ce.status);
                                w.name = c.name;
                                w.status = c.stud;
                                w.class1 = c.class1;
                                w.class2 = ce.course;
                                w.attend = "未到課";
                                w.data = Date;
                                db.rollcallTable_1.Add(w);
                                db.SaveChanges();
                            }
                        }
                    }
                }

            }

            ///重置昨天的暫時點名表
            List<rollcallTable_1> we = db.rollcallTable_1.ToList();
            foreach (var wq in we)
            {
                if (wq.data != Date)
                {
                    db.rollcallTable_1.Remove(wq);
                    db.SaveChanges();
                }
            }
            //課程點名單各科顯示
            //國文課就去國文方法

            if (a != null)//有案連接才會才顯示課程再傳給前端
            {
                List<rollcallTable_1> wa = db.rollcallTable_1.ToList();
                ViewBag.data1 = wa;//課程名單
                ViewBag.data2 = b;///課程代號
            }
            if (course2 == "ch2")
            {
                return RedirectToAction("ch2", new { time, status });
            }
            //英文
            if (course2 == "en1")
            {
                return RedirectToAction("en1", new { time, status });
            }
            //數學
            if (course2 == "ma3")
            {
                return RedirectToAction("ma3", new { time, status });
            }
            return View();
        }
        ///全部修課名單
        public ActionResult ch2()//從view案國文後跳來這裡
        {
            string a = "2";
            string b = "ch2";
            var we = Request.UrlReferrer.Segments[2];
            if (we == "studentmenu")
            {
                return RedirectToAction("studentmenu", new { a, b });
            }
            if (we == "ange")
            {
                return RedirectToAction("studentmenu", new { a, b });

            }
            if (we == "delsy")
            {
                return RedirectToAction("studentmenu", new { a, b });

            }
            else
            {
                return RedirectToAction("exit1", new { a, b });
            }
        }

        public ActionResult ma3()
        {

            string a = "3";
            string b = "ma3";
            var we = Request.UrlReferrer.Segments[2];
            if (we == "studentmenu")
            {
                return RedirectToAction("studentmenu", new { a, b });
            }
            if (we == "ange")
            {
                return RedirectToAction("studentmenu", new { a, b });

            }
            if (we == "delsy")
            {
                return RedirectToAction("studentmenu", new { a, b });

            }
            else
            {
                return RedirectToAction("exit1", new { a, b });
            }
        }

        public ActionResult en1()
        {

            string a = "1";
            string b = "en1";
            var we = Request.UrlReferrer.Segments[2];
            if (we == "studentmenu")
            {
                return RedirectToAction("studentmenu", new { a, b });
            }
            if (we == "ange")
            {
                return RedirectToAction("studentmenu", new { a, b });

            }
            if (we == "delsy")
            {
                return RedirectToAction("studentmenu", new { a, b });

            }
            else
            {
                return RedirectToAction("exit1", new { a, b });
            }
        }

        //總紀錄
        public ActionResult one1(string class2, string select2)
        {
            ViewBag.select1 = class2;
            ViewBag.select2 = select2;
            if (Session["name"] == null)
            {
                return RedirectToAction("register", "one");
            }
            List<string> a = new List<string>();
            List<classromTable_1> wx = db.classromTable_1.ToList();
            string te = Convert.ToString(Session["name"]);
            teacher1 t22 = db.teacher1.FirstOrDefault(t2 => t2.teachername == te);
            if (t22 != null)
            {
                ViewBag.teh = t22.teacher;
            }
            ViewBag.data4 = wx;
            ////抓取點名總紀錄裡的日期
            List<allrollcallTable_1> wr = db.allrollcallTable_1.ToList();
            string i = "";
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
            ViewBag.data1 = a;
            return View();
        }
        ///搜索問題
        [HttpPost]
        public ActionResult one2(string select1, string select2, string class2)
        {
            List<classromTable_1> wx = db.classromTable_1.ToList();
            ViewBag.data4 = wx;
            List<string> a = new List<string>();
            List<allrollcallTable_1> wr = db.allrollcallTable_1.ToList();
            List<allrollcallTable_1> wb = db.allrollcallTable_1.ToList();
            classromTable_1 zw = db.classromTable_1.FirstOrDefault(t => t.Course == select1);
            if (zw != null)
            {
                ViewBag.classroom = zw.classroom;
            }
            string te = Convert.ToString(Session["name"]);
            teacher1 t22 = db.teacher1.FirstOrDefault(t2 => t2.teachername == te);
            if (t22 != null)
            {
                ViewBag.teh = t22.teacher;
            }
            ViewBag.data3 = wr;
            ViewBag.select1 = select1;
            ViewBag.select2 = select2;
            string i = "";
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
            ViewBag.data1 = a;
            //日期
            return View("one1");
        }

        [HttpPost]
        public ActionResult ange(string[] stud, string name29, string class17, string stud12, string phone54, string[] name1, string course, string re, string email12)
        {
            string b = course;
            //ViewBag.course = b;
            List<classromTable_1> cla = db.classromTable_1.ToList();
            ViewBag.cla = cla;
            List<student1Table_1> st = db.student1Table_1.ToList();
            ViewBag.stu = st;
            List<course1Table_1> coy = db.course1Table_1.ToList();
            ViewBag.coy = coy;
            student1Table_1 e = new student1Table_1();

            student1Table_1 w = db.student1Table_1.FirstOrDefault(t => t.stud == stud12);
            if (w == null)
            {
                foreach (var s in st)
                {
                    foreach (var stud1 in stud)
                    {
                        foreach (var name2 in name1)
                        {
                            if (s.stud == stud1)
                            {
                                if (s.name == name2)
                                {
                                    if (stud12 != "")
                                    {
                                        s.stud = stud12;

                                    }
                                    if (name29 != "")
                                    {
                                        s.name = name29;

                                    }
                                    if (class17 != "")
                                    {
                                        s.class1 = class17;

                                    }
                                    if (phone54 != "")
                                    {
                                        student1Table_1 w6 = db.student1Table_1.FirstOrDefault(t => t.phone == phone54);
                                        if (w6 != null)
                                        {
                                            TempData["messg2"] = "電話與人重複,請重新確認電話";
                                            return RedirectToAction("studentmenu", new { b });
                                        }
                                        else
                                        {
                                            s.phone = phone54;
                                        }
                                    }
                                    if (email12 != "")
                                    {
                                        s.allemail = email12;

                                    }
                                    db.SaveChanges();
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                TempData["messg2"] = "學號與人重複,請重新確認更改學號";
                return RedirectToAction("studentmenu", new { b });
            }

            foreach (var c in coy)
            {
                foreach (var stud1 in stud)
                {
                    if (c.status == stud1)
                    {
                        if (stud12 != "")
                        {
                            c.status = stud12;
                            db.SaveChanges();
                        }
                    }
                }
            }
            foreach (var stud1 in stud)
            {
                firstTable_2 z1 = db.firstTable_2.FirstOrDefault(t => t.user == stud1);
                if (z1 != null)
                {
                    if (stud12 != "")
                    {
                        z1.user = stud12;

                    }
                    if (name29 != "")
                    {
                        z1.name = name29;

                    }
                    db.SaveChanges();
                }
            }

            TempData["messg1"] = "更改成功!";
            return RedirectToAction("studentmenu", new { b });
        }
        [HttpPost]
        public ActionResult delsy(string course, string[] status)
        {
            string b = course;
            //var w1 = Request.UrlReferrer.Query;
            //int c = 0;
            //string sb = "";
            //foreach (var zw in w1)
            //{
            //    c += 1;
            //    if (c > 7)
            //    {
            //        sb += zw.ToString();
            //    }
            //}
            List<course1Table_1> coy = db.course1Table_1.ToList();
            foreach (var co in coy)
            {
                foreach (var stud1 in status)
                {
                    if (co.status == stud1)
                    {
                        if (co.course == course)
                        {
                            TempData["messg1"] = "刪除成功!";
                            db.course1Table_1.Remove(co);
                            db.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("studentmenu", new { b });
        }

        [HttpPost]
        public ActionResult adk(string course, string stud, string name, string phone, string select1)
        {
            string b = course;
            student1Table_1 st = db.student1Table_1.FirstOrDefault(t => t.stud == stud);
            if (st == null)
            {
                student1Table_1 w = new student1Table_1();
                w.stud = stud;
                w.name = name;
                w.class1 = select1;
                w.phone = phone;
                db.student1Table_1.Add(w);
                db.SaveChanges();
            }

            List<course1Table_1> co = db.course1Table_1.ToList();
            course1Table_1 wc = new course1Table_1();
            foreach (var w in co)
            {
                if (w.status == stud)
                {
                    if (w.course == course)
                    {
                        TempData["messg2"] = "此人已存在於本課程";
                        return RedirectToAction("studentmenu", new { b });
                    }

                }

            }
            course1Table_1 w1x = db.course1Table_1.FirstOrDefault(t => t.status == stud);
            if (w1x == null)
            {
                wc.status = stud;
                wc.course = course;
                db.course1Table_1.Add(wc);
                db.SaveChanges();
            }


            firstTable_2 f1 = db.firstTable_2.FirstOrDefault(t => t.user == stud);
            if (f1 == null)
            {
                firstTable_2 f = new firstTable_2();
                f.user = stud;
                f.password = "x" + stud + "x";
                f.name = name;
                f.power = "student";
                db.firstTable_2.Add(f);
                db.SaveChanges();
            }
            rollcallTable_1 ro = new rollcallTable_1();
            ro.status = stud;
            ro.name = name;
            ro.class1 = select1;
            ro.class2 = course;
            ro.attend = "未到課";
            ro.data = DateTime.Now.ToString("yyyy-MM-dd");//日期
            db.rollcallTable_1.Add(ro);
            db.SaveChanges();

            return RedirectToAction("studentmenu", new { b });
        }

        public ActionResult teacherregister()
        {
            string user = Convert.ToString(Session["UserID"]);

            firstTable_2 we = db.firstTable_2.FirstOrDefault(t => t.user == user);
            if (we != null)
            {
                ViewBag.naME = we.name;
                teacher1 te = db.teacher1.FirstOrDefault(t => t.teachername == we.name);
                if (te != null)
                {
                    ViewBag.phone = te.phone;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult teacherchange(string name, string phone, string pass)
        {
            string user = Convert.ToString(Session["UserID"]); //登入者的號
            firstTable_2 we = db.firstTable_2.FirstOrDefault(t => t.user == user);
            if (we != null)
            {
                if (name != "" && name != null)
                {
                    teacher1 te = db.teacher1.FirstOrDefault(t => t.teachername == we.name);
                    if (te != null)
                    {
                        te.teachername = name;
                    }
                    we.name = name;
                    TempData["messg3"] = "更改成功";

                }
                if (phone != "" && phone != null)
                {
                    if (phone.Length != 0)
                    {
                        teacher1 te = db.teacher1.FirstOrDefault(t => t.phone == phone);
                        student1Table_1 st = db.student1Table_1.FirstOrDefault(t => t.phone == phone);
                        if (te != null)
                        {
                            if (te.teachername != name)
                            {
                                TempData["messg2"] = "你的電話號碼與人重複";

                            }
                        }
                        else
                        {
                            if (st == null)
                            {
                                te.phone = phone;
                                TempData["messg3"] = "更改成功";
                            }
                        }
                    }

                }

                db.SaveChanges();
            }
            if (pass != null && pass != "")
            {
                firstTable_2 x = db.firstTable_2.FirstOrDefault(t => t.password == pass);
                if (x == null)
                {
                    firstTable_2 a = db.firstTable_2.FirstOrDefault(t => t.user == user);
                    if (a != null)
                    {
                        a.password = pass;
                        TempData["messg3"] = "更改成功";
                    }

                }
                else
                {
                    TempData["messg2"] = "請輸入不一樣的密碼";

                }
            }
            db.SaveChanges();


            return RedirectToAction("teacherregister", "data");
        }
    }
}




