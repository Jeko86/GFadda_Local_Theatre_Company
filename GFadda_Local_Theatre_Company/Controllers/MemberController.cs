using GFadda_Local_Theatre_Company.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;//
using System.Net;//
using System.Threading.Tasks;//
using Microsoft.AspNet.Identity;//
using Microsoft.AspNet.Identity.EntityFramework;//
using GFadda_Local_Theatre_Company.Models.ViewModels;//
using Microsoft.AspNet.Identity.Owin;//


namespace GFadda_Local_Theatre_Company.Controllers
{
    public class MemberController : Controller
    {
        //joins the database to the controllar
        private LocalTheatreCompanyDbContext db = new LocalTheatreCompanyDbContext();

        //Member personal details       
        public ActionResult MemberDetails()
        {
            //find the user logged
            string userId = User.Identity.GetUserId();

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = userManager.FindById(userId);

            return View(user);
        }

        //find the the comment written by the user logged
        public ActionResult MemberCommentList()
        {
            var comments = db.Comments
                .Include(c => c.Post)
                .Include(c => c.User);

            var userId = User.Identity.GetUserId();

            comments = comments.Where(c => c.UserId == userId);

            return View(comments.ToList());
        }

        //it allow to display the logged user's comment dettails
        public ActionResult CommentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        //Delete the selected logged user's comments
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);

            db.Comments.Remove(comment);
            TempData["AllertMessage"] = "Comment Deleted!";
            db.SaveChanges();

            return RedirectToAction("MemberCommentList");
        }

        //allow to edit the loggin member details
        [HttpGet]
        public ActionResult EditMemberDetails(string id)
        {

            string username = User.Identity.Name;

            User member = db.Users.FirstOrDefault(m => m.UserName.Equals(username));

           
            return View(new EditiMemberViewModel
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                City = member.City,
                Street = member.Street,
                PostCode = member.PostCode,
                Email = member.Email,               
            });
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMemberDetails([Bind(Include ="FirstName, LastName, City, Street,PostCode,Email, UserName")] EditiMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;

                User member = db.Users.FirstOrDefault(m => m.UserName.Equals(username));

                member.FirstName = model.FirstName;
                member.LastName = model.LastName;
                member.City = model.City;
                member.Street = model.Street;
                member.PostCode = model.PostCode;
                member.Email = model.Email;
                

                db.Entry(member).State = EntityState.Modified;

                TempData["AllertMessage"] = "Data successfully changed!";

                db.SaveChanges();

                return RedirectToAction("EditMemberDetails");

            }
            return View(model);
        }

    }
}
