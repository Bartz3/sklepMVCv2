using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using sklepMVCv2.Models;

namespace sklepMVCv2.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders


        public ActionResult Summary()
        {
            List<Product> userCart = TempData["userCart"] as List<Product>;
            ViewBag.User=User.Identity.GetUserName();
            string[] paymentStrings = { "Za pobraniem", "BLIK", "Karta kredytowa", "Przelew tradycyjny","PayPal" };
            ViewBag.paymentMethod = new SelectList(paymentStrings);



            return View(userCart);
        }
        
        public ActionResult confirmSummary(FormCollection form)
        {

            string xd = form["paymentMethod"].ToString();
            List<Product> userCart = TempData["userCart"] as List<Product>;
            Order order = new Order();
            OrderProduct orderProduct = new OrderProduct();
            decimal totalPrice = 0;
            if (userCart != null)
                foreach (var item in userCart)
                {
                    totalPrice += item.Price;
                    orderProduct.Product = item;
                    orderProduct.Amount = 1;
                    //orderProduct.Order =;
                }

            order.TotalPrice = totalPrice;
            order.Status = "Przyjęte";
            //order.OrderProduct =;
            //order.OrderID =;
            //order.PaymentMethod = form["paymentMethod"].ToString();


            order.User = getUser();
            ;

            return View("Products/Index");
        }

        public ApplicationUser getUser()
        {
            ApplicationUser user;
            var userId = User.Identity.GetUserId(); // Logged user ID
            user= db.Users.Find(userId);
            return user;
        }
        public ActionResult Index()
        {
            return View(db.Order.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,Date,Status,UserID,TotalPrice,PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,Date,Status,UserID,TotalPrice,PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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
