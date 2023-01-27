using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace sklepMVCv2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

 
        [MaxLength(100)]
        [Display(Name = "Imię")]    
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
       
        [MaxLength(100)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
       
        [MaxLength(100)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name="Numer mieszkania")]
        public int HouseNumber { get; set; }
       
        [MaxLength(10)]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        public virtual ICollection<Complaint> Complaint { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MyDB2", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryProducts> CategoryProducts { get; set; }
        public virtual DbSet<Complaint> Complaint { get; set; }
        public virtual DbSet<ExtraFile> ExtraFile { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<Vat> Vat { get; set; }
        public virtual DbSet<OrderProduct> OrderProduct { get; set; }
        public virtual DbSet<VisitCount> VisitCount { get; set; }

        //public System.Data.Entity.DbSet<sklepMVCv2.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<sklepMVCv2.Models.ApplicationUser> ApplicationUsers { get; set; }

    }

    //Klasa dodana
    public class IdentityManager
    {
        public RoleManager<IdentityRole> LocalRoleManager
        {
            get
            {
                return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
        }


        public UserManager<ApplicationUser> LocalUserManager
        {
            get
            {
                return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            }
        }


        public ApplicationUser GetUserByID(string userID)
        {
            ApplicationUser user = null;
            UserManager<ApplicationUser> um = this.LocalUserManager;

            user = um.FindById(userID);

            return user;
        }


        public ApplicationUser GetUserByName(string email)
        {
            ApplicationUser user = null;
            UserManager<ApplicationUser> um = this.LocalUserManager;

            user = um.FindByEmail(email);

            return user;
        }


        public bool RoleExists(string name)
        {
            var rm = LocalRoleManager;

            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = LocalRoleManager;
            var idResult = rm.Create(new IdentityRole(name));

            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = LocalUserManager;
            var idResult = um.Create(user, password);

            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = LocalUserManager;
            var idResult = um.AddToRole(userId, roleName);

            return idResult.Succeeded;
        }


        public bool AddUserToRoleByUsername(string username, string roleName)
        {
            var um = LocalUserManager;

            string userID = um.FindByName(username).Id;
            var idResult = um.AddToRole(userID, roleName);

            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var um = LocalUserManager;
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }
    }



}