using GFadda_Local_Theatre_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;//to add
using Microsoft.AspNet.Identity;
using GFadda_Local_Theatre_Company.Models.ViewModels;
using System.Data;//to add
using System.Net;

namespace GFadda_Local_Theatre_Company.Controllers
{
    public class BlogController : Controller
    {        
        //links the controller to the database 
        private LocalTheatreCompanyDbContext db = new LocalTheatreCompanyDbContext();

        //display a list of past by category 
        public ActionResult Post()
        {
            List<Post> posts = db.Posts.
                Include(p => p.Category).
                Include(p => p.Staff).
                Include(p => p.Comments).                
                Where(p => p.IsApproved == true).
                Where(p => p.CategoryId == 1).   // it will display Communication only           
                OrderByDescending(p => p.DatePosted).
                ToList();

            
            return View(posts);
            
        }

        //display a list of post by category 
        public ActionResult Movies()
        {

            List<Post> movies = db.Posts.
                Include(p => p.Category).
                Include(p => p.Staff).
                Where(p => p.IsApproved == true).
                Where(p => p.CategoryId == 2).// it will display Movie only 
                OrderByDescending(p => p.DatePosted).
                ToList();

            return View(movies);
        }


        //filtering the comments to inclute only the comments relate to each communication
        public ActionResult Comments(int? id)
        {
            //gets all comments and filters for only those with PostID == id
            List<Comment> comments = db.Comments.
                Include(c => c.Post).
                Include(c => c.User).
                Where(c => c.PostId == id).Where(c => c.IsApproved == true).
                ToList();

            
            foreach (var item in comments)
            {
                db.Entry(item).Reload();
            }

            //it will allow to display the related post in the view 
            PostCommentsViewModel postAndcomments = new PostCommentsViewModel
            {
                //get the posts fron the database which include FK Staff 
                Post = db.Posts.Include(p=>p.Staff).SingleOrDefault(p=>p.PostId == id),
                Comments = comments
            };
            
            return View(postAndcomments);
        }

        //display the the details of each post
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult PostDetails(int? id)
        {
            Post post = db.Posts.Find(id);

            var staff = db.Staffs.Find(post.UserId);

            var category = db.Categories.Find(post.CategoryId);

            post.Category = category;

            post.Staff = staff;

            return View(post);
        }

        //**************************

        //create post's comment 
        [Authorize(Roles = "Admin, Staff, Member")]
        public ActionResult CreateComment(int id)
        {
            //get post from the database 
            Post post = db.Posts.Include(p => p.Staff).SingleOrDefault(p => p.PostId == id);

            CreateCommentViewModel createComment = new CreateCommentViewModel()
            {
                Post = post                
            };

            return View(createComment);       
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(int id, [Bind(Include = "CommentBody")] CreateCommentViewModel comment)
        {
           
            //if paramenter posted is not null
            if (ModelState.IsValid)
            {
                //create comment 
                Comment com = new Comment
                {
                    DatePosted = DateTime.Now,
                    IsApproved = false,
                    UserId = User.Identity.GetUserId(),
                    PostId = id,    
                    CommentBody = comment.CommentBody

                };

                //the new comment to the database 
                db.Comments.Add(com);
                
                //TempData is used to transfer data from view to controller
                TempData["AllertMessage"] = "Comment successfully added! it will be published once approved";
                db.SaveChanges();

                return RedirectToAction("Post");
            }
            

            Post post = db.Posts.Include(p => p.Staff).SingleOrDefault(p => p.PostId == id);
            comment.Post = post;
           
            return View(comment);
            
        }

        //this method will delete a post by id
        public ActionResult DeletePostBlog(int? id)
        {
            //if id is null then return a bad request error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find a post in the Posts tables by id
            Post post = db.Posts.Find(id);

            //find post category in  the database 
            var category = db.Categories.Find(post.CategoryId);

           
            post.Category = category;

            //if the post is a null object            
            if (post == null)
            {
                //then return a not found error message
                return HttpNotFound();
            }

            return View(post);
        }

        
        [HttpPost, ActionName("DeletePostBlog")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find post by id in Posts tables
            Post post = db.Posts.Find(id);

            //remove post from Posts table
            db.Posts.Remove(post);

            TempData["AllertMessage"] = "Post successfully Delete!";
            //save changes in the database
            db.SaveChanges();
           
            return RedirectToAction("Post");

        }
    }
}