namespace ServiceApi.Server.WebApi.Filters
{
	using System;
	using System.Security.Principal;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http.Filters;

	/// <summary>
	/// An authentication filter that authenticates using the current windows principal
	/// </summary>
	public class StaticAuthenticationAttribute : Attribute, IAuthenticationFilter
	{
		public bool AllowMultiple => false;

		public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			var identity = WindowsIdentity.GetCurrent();
			var principal=new WindowsPrincipal(identity);

			context.Principal = principal;
			await Task.FromResult(0);
		}

		public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			await Task.FromResult(0);
		}
	}
}
