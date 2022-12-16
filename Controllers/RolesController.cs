using sklepMVCv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sklepMVCv2.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public string Create()
        {
            IdentityManager im = new IdentityManager();

            im.CreateRole("Moderator");
            im.CreateRole("User");

            return "OK";
        }
    }
}