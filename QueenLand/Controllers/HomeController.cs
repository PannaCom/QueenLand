using QueenLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueenLand.Controllers
{
    public class HomeController : Controller
    {
        private queenlandEntities db = new queenlandEntities();
        public ActionResult Index()
        {
            try
            {
                var hp = (from sl in db.slides select sl).OrderBy(o => o.no).ThenByDescending(o => o.id).Take(20);
                var rshp = hp.ToList();
               
                string slide = "";
                for (int h = 0; h < rshp.Count; h++)
                {
                    slide += "<li data-transition=\"fade\" data-slotamount=\"7\" data-masterspeed=\"1500\">";
                    slide += "<img src=\"" + Config.domain + "/" + rshp[h].image + "\" style=\"opacity:0;\" alt=\"slidebg1\"  data-bgfit=\"cover\" data-bgposition=\"left bottom\" data-bgrepeat=\"no-repeat\">";
                    //slide += " <div class=\"textlink\"><span style=\"text-shadow: 0 0 5px #0026ff, 0 0 7px #0026ff;margin: 10 10 10 10;color:#fff;display: block;font-size:18px;line-height: 1;-webkit-margin-before: 0.67em;-webkit-margin-after: 0.67em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;font-weight: bold;\">" + rshp[h].caption + "</span><b><a href=\"" + rshp[h].link + "\" style=\"color:#fff;font-weight:bold;font-size:12px;\">" + rshp[h].linktext + "</a></b></div>";
                    slide += " <div class=\"caption sft revolution-starhotel smalltext\" data-x=\"35\" data-y=\"30\" data-speed=\"800\" data-start=\"1700\" data-easing=\"easeOutBack\">";
                    slide += " <span style=\"text-shadow: 0 0 25px #000, 0 0 27px #000;margin: 10px 0;color:#fff;display: block;font-size:28px;line-height: 1;-webkit-margin-before: 0.67em;-webkit-margin-after: 0.67em;-webkit-margin-start: 0px;-webkit-margin-end: 0px;font-weight:bold;\">" + rshp[h].slogan + "</span><br>";
                    slide += " </div></li>";
                }
                ViewBag.slide = slide;
            }
            catch (Exception ex)
            {
            }
            //Du an moi
            try
            {
                var p = (from q in db.projects select q).OrderBy(o => o.no).ThenByDescending(o=>o.id).Take(4);
                var prs = p.ToList();
                string projects = "";// "<div class=\"item\" style=\"width:100%;display:block;position:relative;float:left;background-color:#FFCA08;\"><table width=\"100%\" align=center><tr><td align=center>";//<table width=\"100%\"><tr>
                for (int j = 0; j < prs.Count; j++)
                {
                    ///hotel/" + Config.unicodeToNoMark(prs[j].name) + "-" + ViewBag.fromdate + "-" + ViewBag.todate + "-" + prs[j].id + "
                    projects += "<div class=\"itemprojecthome\"><a href=\"" + Config.domain + "/projects/" + Config.unicodeToNoMark(prs[j].name) + "-" + prs[j].id + "\"><img src=\"" + Config.domain + "/" + prs[j].image + "\" width=\"100%\" height=\"116\" alt=\"" + prs[j].name + "\"><br><span style=\"font-weight:bold;text-align:center;font-size:14px;color:yellow;\">" + prs[j].name.Trim() + "</span></a></div>";
                }
                //projects += "</td></tr></table></div>";//</tr></table>
                ViewBag.projects = projects;
            }
            catch (Exception ex2)
            {
            }
            //Tin tuc
            try
            {
                var p2 = (from q in db.news select q).OrderByDescending(o => o.id).Take(6);
                var prs2 = p2.ToList();
                string news = "";
                string link="";
                for (int j = 0; j < prs2.Count; j++)
                {
                    link = "/news/details/"+Config.unicodeToNoMark(prs2[j].title)+"-"+prs2[j].id;
                    ///hotel/" + Config.unicodeToNoMark(prs[j].name) + "-" + ViewBag.fromdate + "-" + ViewBag.todate + "-" + prs[j].id + "
                    news += "<div class=\"col-sm-4 single\" style=\"height:345px;\">";
                    news +=" <div ><a href=\""+link+"\"><img src=\""+Config.domain+prs2[j].image+"\" alt=\""+prs2[j].title+"\" class=\"img-responsive\" /></a>";
                    news +="  <div class=\"mask\">";			
                    news +="   <div class=\"main\">";
                    news += "      <a href=\"" + link + "\"><b>" + prs2[j].title + "</b></a>";
                    news += "      <p>" + prs2[j].des + "</p>";
                    news +="    </div>";
                    news +=" </div>";
                    news +=" </div>";
                    news +=" </div>";

                }
                ViewBag.news = news;
            }
            catch (Exception ex2)
            {
            }
            return View();
        }

        public ActionResult About()
        {
            
            try
            {
                ViewBag.menuleft = Config.getProjectMenu();
                var p = db.abouts.FirstOrDefault().fullcontent;
                ViewBag.content = p;
            }
            catch (Exception ex) { 
                
            }
            return View();
        }

        public ActionResult Contact()
        {
             try
            {
                ViewBag.menuleft = Config.getProjectMenu();
                var p = db.contacts.FirstOrDefault().fullcontent;
                ViewBag.content = p;
            }
            catch (Exception ex) { 
                
            }
             return View();

        }
    }
}
