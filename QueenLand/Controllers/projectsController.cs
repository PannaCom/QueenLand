using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueenLand.Models;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QueenLand.Controllers
{
    public class projectsController : Controller
    {
        private queenlandEntities db = new queenlandEntities();

        //
        // GET: /projects/

        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View(db.projects.ToList());
        }

        //
        // GET: /projects/Details/5

        public ActionResult Details(int id = 0)
        {
            //Lấy ra menu bên trái
            var mn = (from p in db.projects
                      join q in db.projectitems on p.id equals q.projectid
                      orderby p.id
                      select new
                      {
                          image=p.image,projectid=p.id,projectname=p.name,q.itemname,itemid=q.id
                      }).OrderBy(o=>o.projectid).ToList();
            string projectname="";
            string imageMain = "";
            string menuleft = "";
            string link = "";
            string preMenu="";//Mỗi Menu có nhiều Menu item khác nhau, do vậy đọc lần lượt nếu sang Menu mới thì cập nhật item
            for (int i = 0; i < mn.Count; i++) {
                if (mn[i].projectname != preMenu) {
                    preMenu = mn[i].projectname;
                    link = "/projects/" + Config.unicodeToNoMark(mn[i].projectname) + "-" + mn[i].projectid;
                    menuleft += "<div><a href=\"" + link + "\"><b>" + mn[i].projectname.ToUpperInvariant() + "</b></a></div>";
                }
                link = "/projects/" + Config.unicodeToNoMark(mn[i].itemname) + "/" + Config.unicodeToNoMark(mn[i].projectname) + "-" + mn[i].itemid;
                menuleft += "<div>&nbsp;&nbsp;-<a href=\"" + link + "\">" + mn[i].itemname.ToUpperInvariant() + "</a></div>";
                if (mn[i].projectid==id){
                    projectname=mn[i].projectname;
                    imageMain = mn[i].image;
                }
            }
            ViewBag.menuleft = menuleft;
            try
            {
                //Tìm ra item menu đầu tiên của Project ấy
                int minItemId = (int)db.projectitems.Where(o => o.projectid == id).Min(o => o.id);
                //Lấy ra content của nó để hiển thị
                var content = db.projectcontents.Where(o => o.projectid == id).Where(o => o.itemid == minItemId).FirstOrDefault();
                ViewBag.content = "<h1>"+content.title+"</h1>"+content.fullcontent;
                ViewBag.des = content.des;
                ViewBag.image = Config.domain + imageMain;
                ViewBag.url = Config.domain + "projects/" + Config.unicodeToNoMark(projectname) + "-" + id;
                ViewBag.title = projectname;
            }
            catch (Exception ex) { 
            }
            return View();
        }
        
        //
        // GET: /projects/Create

        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View();
        }

        //
        // POST: /projects/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(project project)
        {
            if (ModelState.IsValid)
            {
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcess(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.ProjectImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + Config.genCode());
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            for (int i = 0; i < countFile; i++)
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                Request.Files[i].SaveAs(fullPath);
                break;
            }
            string ok = resizeImage(Config.imgWidthProject, Config.imgHeightProject, fullPath, Config.ProjectImagePath + "/" + nameFile);
            return Config.ProjectImagePath + "/" + nameFile;
        }
        public string resizeImage(int maxWidth, int maxHeight, string fullPath, string path)
        {

            var image = System.Drawing.Image.FromFile(fullPath);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratioX);
            var newHeight = (int)(image.Height * ratioY);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            image.Dispose();

            string fileRelativePath = path;// "newsizeimages/" + maxWidth + Path.GetFileName(path);
            newImage.Save(HttpContext.Server.MapPath(fileRelativePath), newImage.RawFormat);
            return fileRelativePath;
        }
        //
        // GET: /projects/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // POST: /projects/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        //
        // GET: /projects/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // POST: /projects/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            project project = db.projects.Find(id);
            db.projects.Remove(project);
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