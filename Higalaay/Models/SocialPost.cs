using System;

namespace SharpDevelopMVC4.Models
{
	public class SocialPost
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string ImagePath { get; set; }		
		public bool IsPublic { get; set; }
		//public DateTime? PostDate { get; set; }
		
		public string UserId { get; set; }
		public string FullName { get; set; }
	}
}
