using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GFadda_Local_Theatre_Company.Models;
using GFadda_Local_Theatre_Company.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.IO;


namespace GFadda_Local_Theatre_Company.Controllers
{
    //inheriting the controller with the account controller, the administrator can register a new staff member 
    [Authorize(Roles = "Admin, Staff")]
    public class AdminController : AccountController
    {
        //access to the database
        private LocalTheatreCompanyDbContext db = new LocalTheatreCompanyDbContext();

        public AdminController() : base()
        {

        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {

        }

        //*********************************************\\
        //***********USERS*ADMINISTRATIONS*************\\
        //*********************************************\\

        //list of all the users on the database 
        public ActionResult Index()
        {
            //displays all the user on the datbase order by firstname
            var users = db.Users.OrderBy(u => u.FirstName).ToList();

            //will send the list in the view
            return View(users);
        }
        
        //display staff dettails
        public ActionResult StaffDetails()
        {

            string userId = User.Identity.GetUserId();

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = userManager.FindById(userId);

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> ChangeRole(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //cant't change your own role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Admin");
            }
            //get user by id
            User user = await UserManager.FindByIdAsync(id);
            //get user'x current role 
            string actualRole = (await UserManager.GetRolesAsync(id)).Single();//only ever a single role

            //get all the roles from the database and store them as a list of selectedListItems
            var items = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = r.Name == actualRole
            }).ToList();

            //build the changeRoelViewModel object including the list of roles
            //and send it to the view displaying the roles in a dropDownList with the user's current role displayed as selected 

            return View(new ChangeRoleViewModel
            {
                UserName = user.UserName,
                Roles = items,
                ActualRole = actualRole,
            });
        }

        //change the user role
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        public async Task<ActionResult> ChangeRoleConfirmed(string id, [Bind(Include = "Role")] ChangeRoleViewModel model)
        {
            //it avoids to chage the login-in User Role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Admin");
            }

            if (ModelState.IsValid)
            {
                User user = await UserManager.FindByIdAsync(id);//get user by id

                //get user's current role 
                string actualRole = (await UserManager.GetRolesAsync(id)).Single();

                //if no data is changed will be not change anything
                if (actualRole == model.Role)
                {
                    return RedirectToAction("Index", "Admin");
                }

                //remove user form their acutal role 
                await UserManager.RemoveFromRoleAsync(id, actualRole);

                //add the user to the new role  
                await UserManager.AddToRoleAsync(id, model.Role);

                //if the user was suspended 
                if (model.Role == "Suspended")
                {
                    //then set isSuspended == true
                    user.IsSuspended = true;

                    //update user's details in the database 
                    await UserManager.UpdateAsync(user);
                }

                //TempData is used to transfer data from view to controller
                TempData["AllertMessage"] = "Role successfully changed!";

                return RedirectToAction("ChangeRole", "Admin");
            }

            return View(model);

        }

        //edit member details
        [HttpGet]
        public ActionResult EditStaffDetails(string id)
        {
            //get the login-in member 
            string username = User.Identity.Name;

            User staff = db.Users.FirstOrDefault(m => m.UserName.Equals(username));


            return View(new EditStaffViewModel
            {
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                City = staff.City,
                Street = staff.Street,
                PostCode = staff.PostCode,
                Email = staff.Email,
                PhoneNumber = staff.PhoneNumber,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaffDetails([Bind(Include = "FirstName, LastName, City, Street,PostCode,Email, UserName, PhoneNumber")] EditStaffViewModel model)
        {

            
            if (ModelState.IsValid)
            {
                
                string username = User.Identity.Name;

                User staff = db.Users.FirstOrDefault(m => m.UserName.Equals(username));
                //insert data to change 
                staff.FirstName = model.FirstName;
                staff.LastName = model.LastName;
                staff.City = model.City;
                staff.Street = model.Street;
                staff.PostCode = model.PostCode;
                staff.Email = model.Email;
                staff.PhoneNumber = model.PhoneNumber;


                db.Entry(staff).State = EntityState.Modified;

                TempData["AllertMessage"] = "Data successfully changed!";

                TempData["AllertMessage"] = "Details successfully Changed!";

                db.SaveChanges();

                return RedirectToAction("EditStaffDetails");

            }

            return View(model);
        }

        //display Users dettails
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View("Details", user);
        }

        //Create new staff member
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateStaff()
        {
            CreateStaffViewModel staff = new CreateStaffViewModel();

            //get all the roles from the database 
            var roles = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            //assign the roles to the employee roles property
            staff.Roles = roles;

            //send the staff model to the view
            return View(staff);
        }

        [HttpPost]
        public ActionResult CreateStaff(CreateStaffViewModel model)
        {
            //if model is not null
            if (ModelState.IsValid)
            {
                //build the employee
                Staff newStaff = new Staff
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    Street = model.Street,
                    City = model.City,
                    PostCode = model.PostCode,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = true,
                    IsSuspended = false,
                    RegistrationDate = DateTime.Now
                };

                //create user, and store in the Database and pass the password to the hashed 
                var result = UserManager.Create(newStaff, model.Password);

                TempData["AllertMessage"] = "User successfully added!";

                //if user was stored in the database successfully
                if (result.Succeeded)
                {
                    //then add user to the role selected
                    UserManager.AddToRole(newStaff.Id, model.Role);

                    return RedirectToAction("Index", "Admin");
                }
            }
            
            return View(model);
        }

        //*********************************************\\
        //*******************POSTS*********************\\
        //*********************************************\\
        //create Movie
        [HttpGet]
        public ActionResult CreateCommunication()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCommunication([Bind(Include = "PostId, Title, Description, IsApproved, CategoryId, UserId")] Post model, HttpPostedFileBase file)
        {
            //if paramenter post is not null
            if (ModelState.IsValid)
            {
                //build post
                model.IsApproved = false;
                model.DatePosted = DateTime.Now;
                model.UserId = User.Identity.GetUserId();
                model.Comments = new List<Comment>();
                model.CategoryId = 1;

                db.Posts.Add(model);

                //TempData is used to transfer data from view to controller
                TempData["AllertMessage"] = "Communication successfully added!";

                db.SaveChanges();

                return RedirectToAction("CreateCommunication");
            }
            return View(model);

        }

        //create Movie
        [HttpGet]
        public ActionResult CreateMovie()
        {
            //get categoryId
            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");

            return View();        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMovie([Bind(Include = "PostId, Title, Description, IsApproved, CategoryId, UserId")] Post model, HttpPostedFileBase file)
        {
            //if paramenter post is not null
            if (ModelState.IsValid)
            {
                //build post
                model.IsApproved = false;
                model.DatePosted = DateTime.Now;
                model.UserId = User.Identity.GetUserId();
                model.Comments = new List<Comment>();
                model.CategoryId = 2;

                db.Posts.Add(model);

                //TempData is used to transfer data from view to controller
                TempData["AllertMessage"] = "Movie successfully added!";

                db.SaveChanges();

                //storing the image file in the image folder 
                if (file != null)
                {
                    

                    //the position of the dot allow to extract image extention
                    int dotPosition = Path.GetFileName(file.FileName).IndexOf(".");

                    //saving the image file extension
                    string fileExtention = Path.GetFileName(file.FileName).Substring(dotPosition);

                    //rebuild the imagefile name by getting the posstID and concatinate the file extension
                    var fileName = model.PostId + fileExtention;

                    //save the image in the folder
                    var path = Path.Combine(Server.MapPath("~/Images/Posters"), fileName);
                    file.SaveAs(path);                   
                }

                return RedirectToAction("CreateMovie");
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);

        }

        //display a list of post to approve only
        [Authorize(Roles = "Admin")]
        public ActionResult DisplayPostToApprove()
        { 
            List<Post> posts = db.Posts.
                Include(p => p.Category).
                Include(p => p.Staff).
                Where(p => p.IsApproved == false).
                OrderByDescending(p => p.DatePosted).
                ToList();

            return View(posts.ToList());
        }

        // dipslay a list of  all the post created
        [Authorize(Roles = "Admin")]
        public ActionResult ListPosts()
        {
            List<Post> posts = db.Posts.
                Include(p => p.Category).
                Include(p => p.Staff).                 
                OrderByDescending(p => p.DatePosted).
                ToList();

            return View(posts.ToList());
        }

        [Authorize(Roles = "Admin")]
        //approve new posts
        public ActionResult ApprovePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find post by Id in the Posts table including FK Category and Staff
            Post post = db.Posts.Include(p=>p.Category).Include(c=>c.Staff).SingleOrDefault(c=>c.PostId==id);

            if (post == null)
            {                
                return HttpNotFound();               
            }

            //inster in a viewbag all the catagory
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);

            return View(post);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovePost(int id, [Bind(Include = "PostId, Title, Description, IsApproved, DatePosted, CategoryId, UserId ")] Post model)
        {

            if (ModelState.IsValid)
            {
                Post post = db.Posts.Include(p => p.Category).Include(p => p.Staff).SingleOrDefault(c => c.PostId == id);
                
                //change the IsAPprove parameter
                post.IsApproved = true;

                //modify data
                db.Entry(post).State = EntityState.Modified;

                TempData["AllertMessage"] = "Post successfully Approved!";
                //change the the changes
                db.SaveChanges();

                return RedirectToAction("ApprovePost");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);
        }

        //*******************       
        //approve new posts      
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //find post by Id in the Posts table
            Post post = db.Posts.Include(p => p.Category).Include(c => c.Staff).SingleOrDefault(c => c.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            //add the catgory options create into a viewbag
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", post.CategoryId);

            return View(post);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, [Bind(Include = " Title, Description")] Post model)
        {

            if (ModelState.IsValid)
            {
                //set the paramenter changes
                Post post = db.Posts.Include(p => p.Category).Include(p => p.Staff).SingleOrDefault(c => c.PostId == id);
                post.Title = model.Title;
                post.Description = model.Description;
                post.DatePosted = DateTime.Now;


                db.Entry(post).State = EntityState.Modified;

                //TempData is used to transfer data from view to controller
                TempData["AllertMessage"] = "Post successfully modify!";

                //save changes
                db.SaveChanges();

                return RedirectToAction("ListPosts");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);
        }
        //******************

        [Authorize(Roles = "Admin")]
        //this method will delete a post by id
        public ActionResult DeletePost(int? id)
        {
            //if id is null then return a bad request error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //first find a post in the Posts tables by id
            Post post = db.Posts.Find(id);

            //next find post category by searching through the Categories table
            //for a category by id which is the foreign key in that post
            var category = db.Categories.Find(post.CategoryId);

            //assign the category to the Category navigational property Ccategory 
            //so we can display the category name
            post.Category = category;

            //if the post is a null object
            //then return a not found error message
            if (post == null)
            {
                return HttpNotFound();
            }

            //otherwise return the Delete view and send the post to the view 
            //so post details can be viwed
            return View(post);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("DeletePost")]//get the action name 
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find post by id in Posts tables
            Post post = db.Posts.Find(id);

            //remove post from Posts table
            db.Posts.Remove(post);

            TempData["AllertMessage"] = "Post succeffully Deleted!";
            //save changes in the database
            db.SaveChanges();

            //redirect the Index action in MemberControl
            return RedirectToAction("ListPosts");

        }

        
        //*********************************************\\
        //*******************COMMENTS******************\\
        //*********************************************\\

        //display the list of all the comments to approve only
        public ActionResult DisplayCommentsToApprove()
        {
            var Comments = db.Comments.Include(c => c.User).Include(c => c.Post).Where(c => c.IsApproved == false).OrderByDescending(c => c.DatePosted);

            return View(Comments.ToList());
        }

        //approve comments 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ApproveComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get commnets from the database 
            Comment comment = db.Comments.Include(c=>c.User).SingleOrDefault(c=>c.CommentId == id);
           

            if (comment == null)
            {
                return HttpNotFound();  
            }
            

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveComment(int id, [Bind(Include = "CommentId, CommentBody,DatePosted, IsApproved")] Comment model)
        {
            if (ModelState.IsValid)
            {
                Comment comment = db.Comments.Include(c => c.User).SingleOrDefault(c => c.CommentId == id);
                //modify the property "IsApproved"
                comment.IsApproved = true;

                //modify database
                db.Entry(comment).State = EntityState.Modified;

                //TempData is used to transfer data from view to controller
                TempData["AllertMessage"] = "Comment succeffully Approved!";

                db.SaveChanges();

                return RedirectToAction("ApproveComment");
            }
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Name", model.PostId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", model.UserId);

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        //this method will delete a comment by id
        public ActionResult DeleteComment(int? id)
        {
            //if id is null then return a bad request error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find a comment in the Comment table by id
            Comment comment = db.Comments.Find(id);

            //if the post is a null object           
            if (comment == null)
            {
                //return a not found error message
                return HttpNotFound();
            }
        
            return View(comment);
        }

 
        [HttpPost, ActionName("DeleteComment")]//get the action name
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            //find post by id in Posts tables
            Comment comment = db.Comments.Find(id);

            //remove post from Posts table
            db.Comments.Remove(comment);

            //TempData is used to transfer data from view to controller
            TempData["AllertMessage"] = "comment succeffully Deleted!";

            //save changes in the database
            db.SaveChanges();

            //redirect the Index action in AdminControl
            return RedirectToAction("DisplayCommentsToApprove");

        }

    }
}