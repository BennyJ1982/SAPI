namespace ServiceApi.Server.WebApi.Routing
{
	using System.Web.Http;
	using System.Web.OData.Routing;

	public interface IRouteFactory
	{
		ODataRoute MapCustomODataServiceRoute(HttpRouteCollection routes, string routeName, string routePrefix);
	}
}