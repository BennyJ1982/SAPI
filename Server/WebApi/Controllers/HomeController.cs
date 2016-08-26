namespace ServiceApi.Server.WebApi.Controllers
{
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Web.Http;

	public class HomeController : ApiController
	{
		/// <summary>
		/// Returns all data sources currently available.
		/// </summary>
		public HttpResponseMessage Get()
		{
			var builder = new StringBuilder();
			builder.Append("<html><body><h1>Welcome to the Service API Spike</h1><ul>");

			builder.Append("<li>");
			builder.Append("<a href=\"../serviceapi\">View service document</a>");
			builder.Append("</li>");
			builder.Append("<li>");
			builder.Append(" (<a href=\"../serviceapi/$metadata\">View metadata document</a>)");
			builder.Append("</li>");

			builder.Append("</ul></body></html>");

			var response = new HttpResponseMessage { Content = new StringContent(builder.ToString()) };
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
			return response;
		}
	}
}
