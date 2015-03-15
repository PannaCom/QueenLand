using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueenLand.Models;
using Newtonsoft.Json;
namespace QueenLand.Controllers
{
    public class projectitemController : Controller
    {
        private queenlandEntities db = new queenlandEntities();

        //
        // GET: /projectitem/

        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View(db.projectitems.OrderBy(o=>o.projectid).ThenBy(o=>o.id).ToList());
        }

        //
        // GET: /projectitem/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectitem projectitem = db.projectitems.Find(id);
            if (projectitem == null)
            {
                return HttpNotFound();
            }
            return View(projectitem);
        }

        //
        // GET: /projectitem/Create

        public ActionResult Create()
        {
            //var p = (from q in db.projects select q).OrderByDescending(o => o.id).Take(1000);
            //string projects="<select class=\"form-control\" name=\"provin\" id=\"provin\">";
            //var rs = p.ToList();
            //for (int i = 0; i < rs.Count; i++) { 
            //}
            //projects += "</select>";
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            return View();
        }
        public string getProjectName(int id) {
            var p = db.projects.Find(id);
            return p.name;
        }
        public string getProjectItemName(int id)
        {
            var p = db.projectitems.Find(id);
            return p.itemname;
        }
        public string getListProject() { 
            var p = (from q in db.projects select q).OrderByDescending(o => o.id).Take(100);
            return JsonConvert.SerializeObject( p.ToList());
        }
        public string getListProjectItem(int projectid) {
            var p = (from q in db.projectitems where q.projectid == projectid select q).OrderBy(o => o.id).Take(100);
            return JsonConvert.SerializeObject(p.ToList());
        }
        
        //
        // POST: /projectitem/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(projectitem projectitem)
        {
            if (ModelState.IsValid)
            {
                db.projectitems.Add(projectitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectitem);
        }

        //
        // GET: /projectitem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectitem projectitem = db.projectitems.Find(id);
            if (projectitem == null)
            {
                return HttpNotFound();
            }
            return View(projectitem);
        }

        //
        // POST: /projectitem/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(projectitem projectitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectitem);
        }

        //
        // GET: /projectitem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            projectitem projectitem = db.projectitems.Find(id);
            if (projectitem == null)
            {
                return HttpNotFound();
            }
            return View(projectitem);
        }

        //
        // POST: /projectitem/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectitem projectitem = db.projectitems.Find(id);
            db.projectitems.Remove(projectitem);
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