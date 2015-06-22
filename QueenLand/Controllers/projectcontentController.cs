using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueenLand.Models;

namespace QueenLand.Controllers
{
    public class projectcontentController : Controller
    {
        private queenlandEntities db = new queenlandEntities();

        //
        // GET: /projectcontent/

        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View(db.projectcontents.OrderBy(o=>o.projectid).ThenBy(o=>o.itemid).ToList());
        }

        //
        // GET: /projectcontent/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectcontent projectcontent = db.projectcontents.Find(id);
            if (projectcontent == null)
            {
                return HttpNotFound();
            }
            return View(projectcontent);
        }
        public ActionResult SinglePage(int id) {
            int parentProjectId = -1;
            try
            {
                //Tìm ra item menu đầu tiên của Project ấy
                int minItemId = id;//(int)db.projectitems.Where(o => o.projectid == id).Min(o => o.id);
                //Lấy ra content của nó để hiển thị
                var content = db.projectcontents.Where(o => o.itemid == minItemId).FirstOrDefault();
                ViewBag.content = "<h1>" + content.title + "</h1>" + content.fullcontent;
                ViewBag.des = content.des;
                //Tìm ra menuitem này thuộc project nào;
                var parentPr = db.projects.Where(o => o.id == content.projectid).FirstOrDefault();
                ViewBag.image = Config.domain + parentPr.image;
                ViewBag.url = Config.domain + "projects/" + Config.unicodeToNoMark(content.title) + "/" + Config.unicodeToNoMark(parentPr.name) + "-" + id;
                ViewBag.title = parentPr.name + " - " + content.title;
                parentProjectId = parentPr.id;

            }
            catch (Exception ex)
            {
            }
            try
            {
                //Lấy ra menu bên trái
                var mn = (from p in db.projects
                          join q in db.projectitems on p.id equals q.projectid
                          orderby p.id
                          select new
                          {
                              no = p.no,
                              image = p.image,
                              projectid = p.id,
                              projectname = p.name,
                              q.itemname,
                              itemid = q.id
                          }).OrderBy(o => o.no).ToList();
                string projectname = "";
                string imageMain = "";
                string menuleft = "";
                string link = "";
                string preMenu = "";//Mỗi Menu có nhiều Menu item khác nhau, do vậy đọc lần lượt nếu sang Menu mới thì cập nhật item
                
                for (int i = 0; i < mn.Count; i++)
                {
                    //if (mn[i].itemid == id) parentProjectId = mn[i].projectid;
                    if (mn[i].projectname != preMenu)
                    {
                        preMenu = mn[i].projectname;
                        link = "/projects/" + Config.unicodeToNoMark(mn[i].projectname) + "-" + mn[i].projectid;
                        menuleft += "<div id=dvmenuview_" + mn[i].projectid + "><a href=\"" + link + "\"><b>" + mn[i].projectname.ToUpperInvariant() + "</b></a>&nbsp;<span class=\"glyphicon glyphicon-plus\" style=\"float:right;cursor:pointer;\" id=menuview_" + mn[i].projectid + " onclick=\"viewMenuItem(" + mn[i].projectid + ")\"></span></div>";
                    }
                    link = "/projects/" + Config.unicodeToNoMark(mn[i].itemname) + "/" + Config.unicodeToNoMark(mn[i].projectname) + "-" + mn[i].itemid;
                    string style = "style=\"display:none;\"";
                    if (mn[i].projectid == parentProjectId) style = "";
                    menuleft += "<div id=dvmenuview_" + mn[i].projectid + "_" + i + " " + style + ">&nbsp;&nbsp;-<a href=\"" + link + "\">" + mn[i].itemname.ToUpperInvariant() + "</a></div>";
                    //if (mn[i].itemid == id)
                    //{
                    //    projectname = mn[i].projectname;
                    //    imageMain = mn[i].image;
                    //}
                }
                ViewBag.menuleft = menuleft;
            }
            catch (Exception ex2) { 
            }
            return View();
        }
        //
        // GET: /projectcontent/Create

        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View();
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcessContent(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.ProjectImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + "-" + Config.genCode());
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            string content = "";
            for (int i = 0; i < countFile; i++)
            {
                nameFile = String.Format("{0}.jpg", filename + "-" + Guid.NewGuid().ToString());
                fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
                content += "<img src=\"" + Config.ProjectImagePath + "/" + nameFile + "\" width=200 height=126>";
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                Request.Files[i].SaveAs(fullPath);
                //break;
            }
            //string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, fullPath, Config.NewsImagePath + "/" + nameFile);
            //return Config.NewsImagePath + "/" + nameFile;
            return content;
        }
        //
        // POST: /projectcontent/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(projectcontent projectcontent)
        {
            if (ModelState.IsValid)
            {
                db.projectcontents.Add(projectcontent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectcontent);
        }

        //
        // GET: /projectcontent/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectcontent projectcontent = db.projectcontents.Find(id);
            if (projectcontent == null)
            {
                return HttpNotFound();
            }
            return View(projectcontent);
        }

        //
        // POST: /projectcontent/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(projectcontent projectcontent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectcontent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectcontent);
        }

        //
        // GET: /projectcontent/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectcontent projectcontent = db.projectcontents.Find(id);
            if (projectcontent == null)
            {
                return HttpNotFound();
            }
            return View(projectcontent);
        }

        //
        // POST: /projectcontent/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectcontent projectcontent = db.projectcontents.Find(id);
            db.projectcontents.Remove(projectcontent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}