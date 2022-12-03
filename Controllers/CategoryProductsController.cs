using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sklepMVCv2.Models;

namespace sklepMVCv2.Controllers
{
    public class CategoryProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoryProducts
        public ActionResult Index()
        {
            var categoryProducts = db.CategoryProducts.Include(c => c.Category).Include(c => c.Product);
            return View(categoryProducts.ToList());
        }

        // GET: CategoryProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryProducts categoryProducts = db.CategoryProducts.Find(id);
            if (categoryProducts == null)
            {
                return HttpNotFound();
            }
            return View(categoryProducts);
        }

        // GET: CategoryProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName");
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name");
            return View();
        }

        // POST: CategoryProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryProductsID,ProductID,CategoryID")] CategoryProducts categoryProducts)
        {
            if (ModelState.IsValid)
            {
                db.CategoryProducts.Add(categoryProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", categoryProducts.CategoryID);
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name", categoryProducts.ProductID);
            return View(categoryProducts);
        }

        // GET: CategoryProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryProducts categoryProducts = db.CategoryProducts.Find(id);
            if (categoryProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", categoryProducts.CategoryID);
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name", categoryProducts.ProductID);
            return View(categoryProducts);
        }

        // POST: CategoryProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryProductsID,ProductID,CategoryID")] CategoryProducts categoryProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", categoryProducts.CategoryID);
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name", categoryProducts.ProductID);
            return View(categoryProducts);
        }

        // GET: CategoryProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryProducts categoryProducts = db.CategoryProducts.Find(id);
            if (categoryProducts == null)
            {
                return HttpNotFound();
            }
            return View(categoryProducts);
        }

        // POST: CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryProducts categoryProducts = db.CategoryProducts.Find(id);
            db.CategoryProducts.Remove(categoryProducts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
