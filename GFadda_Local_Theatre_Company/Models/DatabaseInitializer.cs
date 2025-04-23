using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;//added
using Microsoft.AspNet.Identity; //added
using Microsoft.AspNet.Identity.EntityFramework; //added
using System.Security.Claims;
using System.Threading.Tasks;


namespace GFadda_Local_Theatre_Company.Models
{
    //the class will allow to initialising the database inserting data
    public class DatabaseInitializer : DropCreateDatabaseAlways<LocalTheatreCompanyDbContext>
    {
        protected override void Seed(LocalTheatreCompanyDbContext context)
        {
            base.Seed(context);

            if (!context.Users.Any())
            {
                //create and store roles
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }
                if (!roleManager.RoleExists("Staff"))
                {
                    roleManager.Create(new IdentityRole("Staff"));
                }
                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }
                if (!roleManager.RoleExists("Suspended"))
                {
                    roleManager.Create(new IdentityRole("Suspended"));
                }

                //create some users
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));
                //Super liberal password validation for password for seeds

                var eleonora = new Staff()
                {
                    UserName = "admin@theatre.com",
                    Email = "admin@theatre.com",
                    FirstName = "Eleonora",
                    LastName = "Scoscini",
                    Street = "40 London road",
                    City = "Glasgow",
                    PostCode = "G3 5BA",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now,
                    IsActive = true,
                    IsSuspended = false,
                };
                //main Administrator
                if (userManager.FindByName("admin@theatre.com") == null)
                {
                    //user creation and password
                    userManager.Create(eleonora, "Admin@123");
                    //role assignement
                    userManager.AddToRoles(eleonora.Id, "Admin");
                }

                var james = new Staff()
                {
                    UserName = "james@theatre.com",
                    Email = "james@theatre.com",
                    FirstName = "James",
                    LastName = "Brown",
                    Street = "10 West George Street",
                    City = "Glasgow",
                    PostCode = "G2 2ND",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now,
                    IsActive = true,
                    IsSuspended = false
                };
                //staff 
                if (userManager.FindByName("james@theatre.com") == null)
                {
                    
                    //user creation and password
                    userManager.Create(james, "James@123");
                    //role assignement
                    userManager.AddToRoles(james.Id, "Staff");
                }

                var Giacomo = new Member()
                {
                    UserName = "giacomo@gmail.com",
                    Email = "giacomo@gmail.com",
                    FirstName = "Giacomo",
                    LastName = "Fadda",
                    Street = "50 Argyle street",
                    City = "Glasgow",
                    PostCode = "G3 8TH",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now,
                    IsActive = true,
                    IsSuspended = false
                };

                //Interst members 
                if (userManager.FindByName("giacomo@gmail.com") == null)
                {
                   
                    //user creation and password
                    userManager.Create(Giacomo, "Giacomo@123");
                    //role assignement
                    userManager.AddToRoles(Giacomo.Id, "Suspended");
                }

                var steve = new Member()
                {
                    UserName = "steve@yahoo.com",
                    Email = "steve@yahoo.com",
                    FirstName = "Steve",
                    LastName = "Black",
                    Street = "183 Crossloan Road",
                    City = "Glasgow",
                    PostCode = "G51 3QE",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now,
                    IsActive = true,
                    IsSuspended = false
                };
                //Member
                if (userManager.FindByName("steve@yahoo.com") == null)
                {
                  
                    //user creation and password
                    userManager.Create(steve, "Steve@123");
                    //role assignement
                    userManager.AddToRoles(steve.Id, "Member");
                }
                //save info into the databse
                context.SaveChanges();

                


                var comment1 = new Comment()
                {
                    CommentBody = "Comment number 1",
                    DatePosted = DateTime.Now,
                    IsApproved = true, 
                    User = Giacomo
                };
                //context.Comments.Add(comment1);


 
                var comment2 = new Comment()
                {
                    CommentBody = "Comment number 2",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    User = Giacomo
                };
                //context.Comments.Add(comment2);

                var comment3 = new Comment()
                {
                    CommentBody = "Comment number 3",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    User = Giacomo
                };
               // context.Comments.Add(comment3);


                var comment4 = new Comment()
                {
                    CommentBody = "Comment number 4 ",
                    DatePosted = DateTime.Now,
                    IsApproved = false,
                    User = steve
                };
                //context.Comments.Add(comment4);

                List<Comment> comments = new List<Comment> { comment1, comment2, comment4 };
                List<Comment> comments1 = new List<Comment> { comment3 };

                //***********************************************************************
                //seeding categories table
                //create categories
                var category1 = new Category() { Name = "Communication" };
                var category2 = new Category() { Name = "Movie" };


                //insert the categories created on the Category's table
                context.Categories.Add(category1);
                context.Categories.Add(category2);

                //save info into the databse
                context.SaveChanges();

                //seeding post table
                //create post
                var post1 = new Post()
                {
                    Title = "Communication 1",

                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras pretium, nibh at ullamcorper ornare, risus dolor pretium lorem, tempor faucibus libero dui et nunc. " +
                    "Maecenas a consequat velit. " +
                    "Aenean bibendum ante sed tempus tristique. " +
                    "Phasellus fermentum faucibus volutpat. Quisque non diam congue, vulputate ligula ac, imperdiet lorem. Donec commodo consequat sodales. " +
                    "Phasellus blandit luctus maximus. Aliquam eu porta neque. Donec ultrices vitae sem eget facilisis. " +
                    "Proin nec orci nec mi hendrerit malesuada. Donec fermentum aliquet volutpat. " +
                    "Aenean porttitor ligula nec congue scelerisque. " +
                    "Pellentesque vitae enim id dui sagittis gravida at et turpis.",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    Category = category1,
                    Staff = james,
                    Comments = comments


                };
                context.Posts.Add(post1);

                var post2 = new Post()
                {
                    Title = "Communication 2",

                    Description = "Praesent molestie est eget neque lobortis, sed tristique lacus cursus. " +
                    "Sed ullamcorper massa ac aliquam sodales. Donec dictum ultricies tellus, a tristique justo auctor quis. " +
                    "Vestibulum et urna sed sem bibendum mollis nec sed eros. Vestibulum congue tortor eget iaculis porta. " +
                    "Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Maecenas vitae facilisis nibh. " +
                    "Integer sed nisl nulla. Cras semper congue metus non ornare. Donec congue euismod odio vitae accumsan. Sed tempor dictum mi varius tincidunt. " +
                    "Vestibulum mi risus, congue quis fermentum ac, lobortis vel lacus. " +
                    "Mauris efficitur, tortor sit amet condimentum consectetur, mauris ante gravida quam, eu dictum lectus lectus eget turpis.",
                    DatePosted = DateTime.Now,
                    IsApproved = false,
                    Category = category1,
                    Staff = eleonora,
                    Comments = comments1

                };
                context.Posts.Add(post2);

                var post3 = new Post()
                {
                    Title = "Communication 3",

                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras pretium, nibh at ullamcorper ornare, risus dolor pretium lorem, tempor faucibus libero dui et nunc. " +
                    "Maecenas a consequat velit. " +
                    "Aenean bibendum ante sed tempus tristique. " +
                    "Phasellus fermentum faucibus volutpat. Quisque non diam congue, vulputate ligula ac, imperdiet lorem. Donec commodo consequat sodales. " +
                    "Phasellus blandit luctus maximus. Aliquam eu porta neque. Donec ultrices vitae sem eget facilisis. " +
                    "Proin nec orci nec mi hendrerit malesuada. Donec fermentum aliquet volutpat. " +
                    "Aenean porttitor ligula nec congue scelerisque. " +
                    "Pellentesque vitae enim id dui sagittis gravida at et turpis.",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    Category = category1,
                    Staff = james,
                };
                context.Posts.Add(post3);

                //Insert some Post Movies

                var post4 = new Post()
                {
                    Title = "Moulin Rouge",
                    Description = "Praesent molestie est eget neque lobortis, sed tristique lacus cursus. " +
                    "Sed ullamcorper massa ac aliquam sodales. Donec dictum ultricies tellus, a tristique justo auctor quis. " +
                    "Vestibulum et urna sed sem bibendum mollis nec sed eros. Vestibulum congue tortor eget iaculis porta. " +
                    "Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Maecenas vitae facilisis nibh. " +
                    "Integer sed nisl nulla. Cras semper congue metus non ornare. Donec congue euismod odio vitae accumsan. Sed tempor dictum mi varius tincidunt. " +
                    "Vestibulum mi risus, congue quis fermentum ac, lobortis vel lacus. " +
                    "Mauris efficitur, tortor sit amet condimentum consectetur, mauris ante gravida quam, eu dictum lectus lectus eget turpis.",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    Staff = eleonora,
                    File = "/Images/Posters/poster.jpg",
                    Category = category2
                };
                context.Posts.Add(post4);

                var post5 = new Post()
                {
                    Title = "Hamilton",
                    Description = "Praesent molestie est eget neque lobortis, sed tristique lacus cursus. " +
                    "Sed ullamcorper massa ac aliquam sodales. Donec dictum ultricies tellus, a tristique justo auctor quis. " +
                    "Vestibulum et urna sed sem bibendum mollis nec sed eros. Vestibulum congue tortor eget iaculis porta. " +
                    "Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Maecenas vitae facilisis nibh. " +
                    "Integer sed nisl nulla. Cras semper congue metus non ornare. Donec congue euismod odio vitae accumsan. Sed tempor dictum mi varius tincidunt. " +
                    "Vestibulum mi risus, congue quis fermentum ac, lobortis vel lacus. " +
                    "Mauris efficitur, tortor sit amet condimentum consectetur, mauris ante gravida quam, eu dictum lectus lectus eget turpis.",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    Staff = james,
                    File = "/Images/Posters/poster1.jpg",
                    Category = category2
                };
                context.Posts.Add(post5);

                var post6 = new Post()
                {
                    Title = "Romeo & Juliet",
                    Description = "Praesent molestie est eget neque lobortis, sed tristique lacus cursus. " +
                    "Sed ullamcorper massa ac aliquam sodales. Donec dictum ultricies tellus, a tristique justo auctor quis. " +
                    "Vestibulum et urna sed sem bibendum mollis nec sed eros. Vestibulum congue tortor eget iaculis porta. " +
                    "Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Maecenas vitae facilisis nibh. " +
                    "Integer sed nisl nulla. Cras semper congue metus non ornare. Donec congue euismod odio vitae accumsan. Sed tempor dictum mi varius tincidunt. " +
                    "Vestibulum mi risus, congue quis fermentum ac, lobortis vel lacus. " +
                    "Mauris efficitur, tortor sit amet condimentum consectetur, mauris ante gravida quam, eu dictum lectus lectus eget turpis.",
                    DatePosted = DateTime.Now,
                    IsApproved = true,
                    Staff = james,
                    File = "/Images/Posters/poster2.jpg",
                    Category = category2
                };
                context.Posts.Add(post6);

                context.SaveChanges();
            }
        }
    }
}