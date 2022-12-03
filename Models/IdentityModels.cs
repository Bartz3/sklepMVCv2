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

       // [Required]
        [MaxLength(100)]
        [Display(Name = "Imię")]    
        public string Name { get; set; }
       // [Required]
        [MaxLength(100)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
       //[Required]

        public int AddressID { get; set; }
        public virtual Address Address { get; set; }
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

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryProducts> CategoryProducts { get; set; }
        public virtual DbSet<Complaint> Complaint { get; set; }
        public virtual DbSet<ExtraFile> ExtraFile { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        //public virtual DbSet<ApplicationUser> User { get; set; }

        //public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vat> Vat { get; set; }
        public virtual DbSet<OrderProduct> OrderProduct { get; set; }
        public virtual DbSet<VisitCount> VisitCount { get; set; }
    }
}