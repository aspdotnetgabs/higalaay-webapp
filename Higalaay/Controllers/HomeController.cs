using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpDevelopMVC4.Models;

namespace SharpDevelopMVC4.Controllers
{
    public class HomeController : Controller
    {
    	SdMvc4DbContext _db = new SdMvc4DbContext();
    	string adminId = "admin";
    	
        public ActionResult Index()
        {
        	string userId = Session["UserId"] == null ? string.Empty : Session["UserId"].ToString();
        	string fullName = Session["FullName"] == null ? string.Empty : Session["FullName"].ToString();
        	ViewBag.UserId = userId;
        	ViewBag.FullName = fullName;
        	ViewBag.AdminId = adminId;
        	        	        
        	var newsfeed = _db.SocialPosts
				.Where(x => x.IsPublic // Only show in newsfeed if post is Public
			       || x.UserId == userId // or post of the owner
			       || adminId == userId) // or administrator
        		.ToList();
        	
        	return View(newsfeed);
        }
        
        [HttpGet]
        public ActionResult Post()
        {
        	ViewBag.UserId = Session["UserId"] == null ? string.Empty : Session["UserId"].ToString();
        	ViewBag.FullName = Session["FullName"] == null ? string.Empty : Session["FullName"].ToString();
        	
        	return View();
        }
        
        [HttpPost]
        public ActionResult Post(SocialPost post)
        {        	
        	if(!string.IsNullOrWhiteSpace(post.UserId))
        	{
            	post.IsPublic = false; // Make the post initially viewable only by the owner
            	
        		_db.SocialPosts.Add(post);    
        		_db.SaveChanges();
        	}

        	// Create a session which holds and retain user information.
    		Session["UserId"] = post.UserId;
    		Session["FullName"] = post.FullName;
    		
        	return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MakePublic(int postId)
        {
        	if(Session["UserId"] != null)
        	{
        		// Only administrator can change the privacy setting to Public
            	if(Session["UserId"].ToString() == adminId)
	        	{
	               	var post = _db.SocialPosts.Find(postId);
		        	post.IsPublic = true;
		        	_db.SaveChanges(); 		
	        	}    		
        	}
        
        	return RedirectToAction("Index");
        }
    }
}