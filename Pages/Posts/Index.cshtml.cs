using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NuGet.Common;
using PRN_Project.Filters;
using PRN_Project.Models;

namespace PRN_Project.Pages.Posts
{
	[AuthenticationFilter]
	public class IndexModel : PageModel
	{

		private readonly PRN_Project.Models.FinalProjectContext _context;

		public IndexModel(PRN_Project.Models.FinalProjectContext context)
		{
			_context = context;
		}

		public IList<Post> Post { get; set; } = default!;

		public User User { get; set; } = default!;

		[BindProperty]
		public CreatePostForm CreatePostForm { get; set; }

		public async Task<IActionResult> OnGetAsync(string searchString = "")
		{
			if (Request.Cookies.TryGetValue("token", out string? token))
			{
				string userid = token.Split(".")[2];

				User = await _context.Users.FirstOrDefaultAsync(x => x.UserId == int.Parse(userid));

				searchString = searchString == null ? "" : searchString;
				Post = await _context.Posts
					.Include(p => p.User)
					.Include(p => p.Comments)
					.Where(x => x.Title.Contains(searchString))
					.OrderByDescending(x => x.PostDate)
					.ToListAsync();
				
			}
			return Page();

		}

		public async Task<IActionResult> OnGetLogout()
		{
			await Task.Run(() => Response.Cookies.Delete("token"));
			Console.WriteLine("Delete Cookies");
			return RedirectToPage("/Login");
		}

		public async Task OnPostCreate()
		{
			if (!Request.Cookies.TryGetValue("token", out string? token)) return;

			string userid = token.Split(".")[2];

			User = await _context.Users.FirstOrDefaultAsync(x => x.UserId == int.Parse(userid));
			string formatString = CreatePostForm.Content
			.Replace("<div>", "")
			.Replace("<div bis_skin_checked=\"1\">", "")
            .Replace("<br>", " [br]")
            .Replace("</div>", " [br]")
			.Replace("&nbsp;", " ");

			Console.Clear();
			Console.WriteLine(formatString);
			Post post = new Post()
			{
				UserId = User.UserId,
				Content = formatString,
				PostDate = DateTime.Now,
				Title = formatString.Split("[br]")[0]
			};

			_context.Posts.Add(post);
			await _context.SaveChangesAsync();
			await OnGetAsync();
			Console.WriteLine("Finish Create Post");
		}

		public async Task OnPostComment(string commentContent, string postId)
		{
			if (!Request.Cookies.TryGetValue("token", out string? token)) return;
			string userid = token.Split(".")[2];

			User = await _context.Users.FirstOrDefaultAsync(x => x.UserId == int.Parse(userid));
			string formatString = commentContent
			.Replace("<div>", "")
			.Replace("<div bis_skin_checked=\"1\">", "")
			.Replace("<br>", " [br]")
			.Replace("</div>", " [br]")
			.Replace("&nbsp;", " ");

			Comment comment = new Comment()
			{
				UserId = User.UserId,
				Content = formatString,
				CommentDate = DateTime.Now,
				PostId = int.Parse(postId),
			};
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();
			await OnGetAsync();
		}

		public async Task OnPostDeleteComment(string commentId)
		{
			if (!Request.Cookies.TryGetValue("token", out string? token)) return;
			string userid = token.Split(".")[2];

			User = await _context.Users.FirstOrDefaultAsync(x => x.UserId == int.Parse(userid));
			var comment = await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == int.Parse(commentId));
			if (comment != null)
			{
				if (User.UserId == comment.UserId)
				{
					_context.Comments.Remove(comment);
					await _context.SaveChangesAsync();
				}
			}
			await OnGetAsync();
		}
	}

	public class CreatePostForm
	{
		public string Content { get; set; }
	}

	public class Service
	{
		public string ImageUrl { get; set; }
		public string Name { get; set; }

		public static Service Friend => new Service() { ImageUrl = "https://cdn-icons-png.freepik.com/512/3429/3429193.png", Name = nameof(Friend) };

		public static Service Group => new Service() { ImageUrl = "https://cdn-icons-png.flaticon.com/512/166/166258.png", Name = nameof(Group) };

		public static Service Video => new Service() { ImageUrl = "https://cdn-icons-png.flaticon.com/512/4404/4404094.png", Name = nameof(Video) };
	}

	public class DefaultServices
	{
		private static DefaultServices instance;

		private DefaultServices()
		{
			Services = new List<Service>
			{
				Service.Friend,
				Service.Group,
				Service.Video,
			};
		}
		public static DefaultServices Instance
		{
			get
			{
				return instance ?? (instance = new DefaultServices());
			}
			set
			{
				instance = value;
			}
		}

		public List<Service> Services { get; set; }
	}

	public class Avatar
	{
		public static List<string> Defaults => new List<string>
		{
            "https://img.freepik.com/premium-vector/beauty-girl-avatar-character-simple-vector_855703-380.jpg",
            "https://pics.craiyon.com/2023-07-02/71f24db693644b55bc46850669ccb48b.webp"
        };
	}

}
