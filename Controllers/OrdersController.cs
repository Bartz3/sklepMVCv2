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

        //[HttpPost]
        //public ActionResult Summary(int d)
        //{


        //    return RedirectToAction("confirmSummary");
        //}
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
                    UserID=1337,
                    
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

            //List<OrderProduct> orderProductList = new List<OrderProduct>();
            //OrderProduct orderProduct;

            //int orderId;
            //if (db.Order != null)
            //{
            //    orderId = db.Order.Select(o => o.OrderID).Max() + 1;
            //}
            //else
            //{
            //    orderId = 0;
            //}
            //// Tabela OrderProduct -> OrderProductID - PK, 1.Amount - int, 2.ProductID - id produktu,
            //// 3.OrderID - id zamówienia, 4.Order - zamówienie, Product- produkt
            //decimal totalPrice = 0;

            //foreach (var product in userCart)
            //{
            //    orderProduct = new OrderProduct();
            //    orderProduct.Amount = 1;
            //    orderProduct.ProductID = product.ProductID;
            //    orderProduct.Product = product;
            //    //orderproduct.Order=??
            //    orderProduct.OrderID = orderId;

            //    totalPrice += product.Price * orderProduct.Amount; // Cena koszyka
            //    //db.OrderProduct.Add(orderProduct);
            //}

            ////db.SaveChanges();

            //// Order -> Date, Status, UserID, TotalPrice, PaymentMethod,ShippingMethod, List<OrderProduct> ???, User
            //Order order = new Order();

            //order.Date = DateTime.Now;
            //order.TotalPrice = totalPrice;
            //order.Status = "Przyjęte";
            //order.Status = OrderStatus.Nowe.ToString();
            ////order.OrderProduct =;
            ////order.OrderID =;
            //order.PaymentMethod = form["paymentMethod"];
            //order.ShippingMethod = form["shippingMethod"];

            var product = db.Product.Include(p => p.Vat);

            //order.User = getUser();

            return View("~/Views/Products/Index.cshtml", product.ToList());
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
        public ActionResult Edit([Bind(Include = "OrderID,Date,Status,UserID,TotalPrice,PaymentMethod,ShippingMethod")] Order order)
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
