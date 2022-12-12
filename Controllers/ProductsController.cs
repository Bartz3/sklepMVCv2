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
        public ActionResult UserView()
        {
            var product = db.Product.Include(p => p.Vat);
            return View(product.ToList());
        }

        //Dodawanie do koszyka w ciasteczku
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            // Retrieve the product from the database
            var product = db.Product.Find(id);

            // Retrieve the shopping cart cookie
            var cartCookie = Request.Cookies["ShoppingCart"];
            if (cartCookie == null)
            {
                // Create a new cookie if it doesn't exist
                cartCookie = new HttpCookie("ShoppingCart");
            }

            // Add the product to the cookie
            cartCookie.Values[id.ToString()] = product.Name;

            // Save the cookie
            Response.Cookies.Add(cartCookie);

            // Redirect to the shopping cart page
            return RedirectToAction("ShoppingCart", "Products");
        }
        //Odczytywanie danych z koszyka
        public ActionResult showCart()
        {
            var cartCookie = Request.Cookies["ShoppingCart"];
            if (cartCookie != null)
            {
                // Create a list to store the products
                var products = new List<Product>();

                // Iterate over the cookie values
                foreach (var key in cartCookie.Values.AllKeys)
                {
                    // Retrieve the product from the database using the key (product ID)
                    var product = db.Product.Find(int.Parse(key));
                    if (product != null)
                    {
                        // Add the product to the list
                        products.Add(product);
                    }
                }

                // Pass the list of products to the view
                return View(products);

            }
            return RedirectToAction("ShoppingCart", "Products");
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);

            var categoryProducts = db.CategoryProducts.Include(c => c.Category).Where(p=>p.ProductID==id).Select(p=>p.Category.CategoryName).ToList();

            ViewBag.categories =categoryProducts;
            // var query2 = db.Companies.Where(c => c.Name.ToLower() == company.Name.ToLower());
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.VatID = new SelectList(db.Vat, "VatRate", "VatRate");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Description,Price,SmallImage,Image,Quantity,VatID")] Product product, HttpPostedFileBase imageName)
        {

            if (ModelState.IsValid)
            {
                
                product.Image = new byte[imageName.ContentLength];
                imageName.InputStream.Read(product.Image, 0, imageName.ContentLength);
               

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
        public ActionResult Edit([Bind(Include = "ProductID,Name,Description,Price,SmallImage,Image,Quantity,VatID")] Product product, HttpPostedFileBase imageName)
        {
            if (ModelState.IsValid)
            {
                product.Image = new byte[imageName.ContentLength];
                imageName.InputStream.Read(product.Image, 0, imageName.ContentLength);

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

        public ActionResult goToBucket()
        {
        
            return RedirectToAction("Bucket");
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
