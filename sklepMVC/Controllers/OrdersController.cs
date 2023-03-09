﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using PagedList;
using sklepMVCv2.Models;

namespace sklepMVCv2.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //[Authorize(Roles = "User")]
        //[Authorize(Roles = "Moderator")]
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "User,Moderator,Admin")]
        public ActionResult Summary()
        {
            List<Product> userCart = TempData["userCart"] as List<Product>;
            ViewBag.User = User.Identity.GetUserName();

            string[] paymentStrings = { "Za pobraniem", "BLIK", "Karta kredytowa", "Przelew tradycyjny", "PayPal" };
            ViewBag.paymentMethod = new SelectList(paymentStrings);

            string[] shippingStrings = { "Odbiór w salonie", "Inpost-paczkomaty", "Kurier DPD", "Kurier UPS", "Kurier FedEx", "Poczta Polska" };
            ViewBag.shippingMethod = new SelectList(shippingStrings);


            return View(userCart);
        }

        public void SaveOrderToDB()
        {
            var form = Request.Form;

            
                // Order -> Date, Status, UserID, TotalPrice, PaymentMethod,ShippingMethod, List<OrderProduct> ???, User
                var order = new Order()
                {
                    Date = DateTime.Now,
                    Status = OrderStatus.Nowe.ToString(),
                    PaymentMethod = form["paymentMethod"],
                    ShippingMethod = form["shippingMethod"],
                    User = getUser(),
                    UserID= User.Identity.GetUserId()

        };

                var orderProducts = new List<OrderProduct>();
                orderProducts = GetOrderProducts(order);

                order.TotalPrice = costOfOrder();
                order.OrderProduct = orderProducts;
                 var user = getUser();
                user.Order.Add(order);
                db.Order.Add(order);
                db.SaveChanges();
            
        }
        public decimal costOfOrder() {
            List<Product> userCart = getProductsFromBucket();
            decimal output = 0;
            foreach (var product in userCart)
            {
                output+= product.Price;
            }
            return output;
        }
        public List<OrderProduct> GetOrderProducts(Order order) // Products -> OrderProducts
        {

            OrderProduct orderProduct;
            List<OrderProduct> output=new List<OrderProduct>();

            var userCart = getProductsFromBucket();
            foreach (var product in userCart)
            {
                orderProduct = new OrderProduct();
                orderProduct.Amount = 1;
                orderProduct.ProductID = product.ProductID;
                orderProduct.Product = product;
                orderProduct.Order = order;
                orderProduct.OrderID = order.OrderID;

                output.Add(orderProduct);
                //db.OrderProduct.Add(orderProduct);
            }
            return output;

        }
        public List<Product> getProductsFromBucket()
        {
            var cartCookie = Request.Cookies["ShoppingCart"];
            var userCart = new List<Product>();
            if (cartCookie != null)
            {
                // Create a list to store the products

                // Iterate over the cookie values
                foreach (var key in cartCookie.Values.AllKeys)
                {
                    // Retrieve the product from the database using the key (product ID)
                    var product = db.Product.Find(int.Parse(key));
                    if (product != null)
                    {
                        // Add the product to the list
                        userCart.Add(product);
                    }
                }
                // Pass the list of products to the view
            }
            return userCart;
        }
        
        public ActionResult confirmSummary()
        { 
        
            SaveOrderToDB();
            var products = db.Product.Include(p => p.Vat);
            IPagedList<Product> pagedListOfProducts;
            //order.User = getUser();
            pagedListOfProducts = products.ToList().ToPagedList(1, 9);

            return View("~/Views/Products/UserView.cshtml", pagedListOfProducts);
        }

        public ApplicationUser getUser()
        {
            ApplicationUser user;
            var userId = User.Identity.GetUserId(); // Logged user ID
            user = db.Users.Find(userId);
            
            var address = user.Street;
            return user;
        }

        // GET: Orders
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {   
            var orders = db.Order.OrderByDescending(o => o.Date).ToList();
            
            return View(orders);
        }

        // GET: Orders/Details/5
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "OrderID,Date,Status,UserID,TotalPrice,PaymentMethod,ShippingMethod")] Order order)
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            //string[] status = {OrderStatus.Nowe.ToString(),OrderStatus.W_trakcie_realizacji.ToString(),
            //    OrderStatus.Zrealizowane.ToString(),OrderStatus.Anulowane.ToString() };
            string[] status = { "Nowe", "W_trakcie_realizacji", "Zrealizowane", "Anulowane" };

            ViewBag.status = new SelectList(status);


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
        public void sendMail(string email,string oldStatus,string newStatus,int orderId)
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("kontakt.sklepmvc@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Zmiana statusu zamówienia";
                mail.Body = $"Zamówienie o ID {orderId} zmieniło status z {oldStatus} na {newStatus}.";
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("kontakt.sklepmvc@gmail.com", "fruniobrdioagjjw");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                }
            }
        }
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "OrderID,Date,Status,UserID,TotalPrice,PaymentMethod,ShippingMethod")] Order order)
        {
            var originalOrder = db.Order.Find(order.OrderID);

            if (originalOrder.Status != order.Status)
            {
                ApplicationUser applicationUser = db.Users.Find(originalOrder.UserID);
                var email = applicationUser.Email;
                sendMail(email, originalOrder.Status, order.Status, order.OrderID);
            }

            db.Entry(originalOrder).State = EntityState.Modified;
            db.Entry(originalOrder).CurrentValues.SetValues(order);

 
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
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