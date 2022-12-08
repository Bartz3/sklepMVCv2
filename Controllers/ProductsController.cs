using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using sklepMVCv2.Models;

namespace sklepMVCv2.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Vat);
            return View(product.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            CategoryProducts categoryProducts = db.CategoryProducts.Find(product.ProductID);
            // List<Category> categories = db.Category.Select(c=>c.CategoryName).Where(cp=>cp == product.ProductID);
            
            //List<Category> categories = db.CategoryProducts.Select(cp => cp.Category).Where(c=>c.CategoryID == product.Categor).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.VatID = new SelectList(db.Vat, "VatID", "VatID");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Description,Price,SmallImage,BigImage,Quantity,VatID")] Product product, HttpPostedFileBase bigimgname)
        {

            if (ModelState.IsValid)
            {
                
                product.BigImage = new byte[bigimgname.ContentLength];
                bigimgname.InputStream.Read(product.BigImage, 0, bigimgname.ContentLength);
               

                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VatID = new SelectList(db.Vat, "VatID", "VatID", product.VatID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.VatID = new SelectList(db.Vat, "VatID", "VatID", product.VatID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Description,Price,SmallImage,BigImage,Quantity,VatID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VatID = new SelectList(db.Vat, "VatID", "VatID", product.VatID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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



       /* public ActionResult AddImage(Product model, HttpPostedFileBase bigimgname)
        {
            if (bigimgname != null)
            {
              
            }
            else
            {
                ViewBag.message = "Nie zapisano prawidłowo xd";
                return View();
            }
        }*/
    }
}
