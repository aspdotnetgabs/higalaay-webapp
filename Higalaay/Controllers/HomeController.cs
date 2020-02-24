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
    	
        public ActionResult Index()
        {
        	ViewBag.UserId = Session["UserId"] == null ? string.Empty : Session["UserId"].ToString();
        	ViewBag.FullName = Session["FullName"] == null ? string.Empty : Session["FullName"].ToString();
        	        	        
        	var newsfeed = _db.SocialPosts.ToList();
        	
//			if(ViewBag.UserId != "admin")
//			{
//				newsfeed = newsfeed.Where(x => x.IsPublic).ToList();;
//			}

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
            	post.IsPublic = false;
        		_db.SocialPosts.Add(post);    
        		_db.SaveChanges();
        	}

    		Session["UserId"] = post.UserId;
    		Session["FullName"] = post.FullName;
    		
        	return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MakePublic(int postId)
        {
        	if(Session["UserId"] != null)
        	{
            	if(Session["UserId"].ToString() == "admin")
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