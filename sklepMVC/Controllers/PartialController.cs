using sklepMVCv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace sklepMVCv2.Controllers
{
    public class PartialController :Controller
    {

        public ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ShowCounter()
        {
            var counter = db.VisitCount.Find(1);
            if (HttpContext.Session["Visitor"] == null)
            {
                //Linq query to get city details from the DB
               
                counter.VisitsNumber++;
                HttpContext.Session["Visitor"] = "hehe";
                db.SaveChanges();
            }
            //bind seleclistitems list to to viewBag 
            ViewBag.Counter = counter.VisitsNumber;

            //returning the partial view
            return PartialView();
        }
    }
}