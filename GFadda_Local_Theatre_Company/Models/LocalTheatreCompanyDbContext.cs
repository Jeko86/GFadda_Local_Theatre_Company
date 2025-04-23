using Microsoft.AspNet.Identity.EntityFramework;//added
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;//Added
using System.Security.Claims;//added
using System.Threading.Tasks;//added
using Microsoft.AspNet.Identity;//added

namespace GFadda_Local_Theatre_Company.Models
{
    public class LocalTheatreCompanyDbContext : IdentityDbContext<User>
    {
        //properties to create the related tables when tha app runs
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Staff> Staffs { get; set; }     

        public LocalTheatreCompanyDbContext()
            : base("TheatreConnection", throwIfV1Schema: false)
        {
            //join the databseInitializer class to the database
            Database.SetInitializer(new DatabaseInitializer());
        }

        public static LocalTheatreCompanyDbContext Create()
        {
            return new LocalTheatreCompanyDbContext();
        }
    }
}