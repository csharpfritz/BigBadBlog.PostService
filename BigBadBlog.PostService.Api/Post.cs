using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BigBadBlog.PostService.Api;

public class Post
{

	[Required, MaxLength(100)]
	public string Title { 
		get { return _Title; }
		set {
			_Title = value;
			Slug = string.IsNullOrEmpty(Slug) ? Uri.EscapeDataString(_Title.ToLowerInvariant()) : Slug;
		}
	}
	private string _Title;

	[Required, MaxLength(50)]	
	public string Author { get; set; } 
	
	[Required]
	public DateTime Date { get; set; }

	public string Content { get; set; }

	public string Slug { 
		get { return _Slug; }
		set { _Slug = string.IsNullOrEmpty(value) ? _Slug : value; } 
	}
	private string _Slug;

	[Key]
	public ObjectId _id { get; set; }

}
