using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PRN_Project.Models;
using PRN_Project.Utils;

namespace PRN_Project.Filters
{
	public class AuthenticationFilterAttribute : ResultFilterAttribute
	{

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			if (context.HttpContext.Request.Cookies.TryGetValue("token", out var token) && !string.IsNullOrEmpty(token))
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var _context = serviceProvider.GetRequiredService<FinalProjectContext>();

            string hash = token.Split('.')[0];
            string accountid = token.Split('.')[1];

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId == int.Parse(accountid));
            if (account == null || !Hash.Compare(account.Credential, hash))
            {
                context.HttpContext.Response.Redirect("/Login");
                return;
            }
            await next.Invoke();
        }
        else
        {
            context.HttpContext.Response.Redirect("/Login");
            return;
        }
		}
	}
}
