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
using PagedList;
namespace QueenLand.Controllers
{
    public class newsController : Controller
    {
        private queenlandEntities db = new queenlandEntities();

        //
        // GET: /news/

        public ActionResult Index(int? page)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            var p = (from q in db.news select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult List(int? page)
        {
            ViewBag.menuleft = Config.getProjectMenu();
            //Lấy ra các tin
            var p = (from q in db.news select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult GetDetails(int id = 0)
        {
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.menuleft = Config.getProjectMenu();
            ViewBag.content = "<h1>" + news.title + "</h1>" + news.fullcontent;
            ViewBag.des = news.des;
            ViewBag.image = Config.domain + news.image;
            ViewBag.url = Config.domain + "news/details/" + Config.unicodeToNoMark(news.title) + "-" + id;
            ViewBag.title = news.title;
            return View(news);
        }
        //
        // GET: /news/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        //
        // GET: /news/Create

        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View();
        }

        //
        // POST: /news/Create

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(news news)
        {
            if (ModelState.IsValid)
            {
                news.datetime = DateTime.Now;
                db.news.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        //
        // GET: /news/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        //
        // POST: /news/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(news news)
        {
            if (ModelState.IsValid)
            {
                if (news.datetime == null) news.datetime = DateTime.Now;
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        //
        // GET: /news/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        //
        // POST: /news/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            news news = db.news.Find(id);
            db.news.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcess(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.NewsImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + "-" + Config.genCode());
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
            string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, fullPath, Config.NewsImagePath + "/" + nameFile);
            return Config.NewsImagePath + "/" + nameFile;
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcessContent(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.NewsImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename + "-" + Config.genCode());
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            string content = "";
            for (int i = 0; i < countFile; i++)
            {
                nameFile = String.Format("{0}.jpg", filename + "-" + Guid.NewGuid().ToString());
                fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
                content += "<img src=\"" + Config.NewsImagePath + "/" + nameFile + "\" width=200 height=126>";
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
    }
}