namespace ServiceApi.Server.WebApi.Routing
{
	using System.Web.Http;
	using System.Web.OData.Batch;
	using System.Web.OData.Routing;
	using System.Web.OData.Routing.Conventions;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Repository;

	/// <summary>
	/// A factory which can create and map custom odata routes
	/// </summary>
	public class RouteFactory : IRouteFactory
	{
		private readonly IODataRepository oDataRepository;

		private readonly IMappingLogger logger;

		private const bool EnableBatchSupport = false;

		public RouteFactory(IODataRepository oDataRepository, IMappingLogger logger)
		{
			this.oDataRepository = oDataRepository;
			this.logger = logger;
		}

		public ODataRoute MapCustomODataServiceRoute(HttpRouteCollection routes, string routeName, string routePrefix)
		{
			var routingConventions = ODataRoutingConventions.CreateDefault();
			routingConventions.Insert(0, new MatchAllRoutingConvention(this.logger));

			routePrefix = RemoveTrailingSlash(routePrefix);

			// TODO: batches
			//if (EnableBatchSupport)
			//{
			//	MappHttpBatchRoute(routes, new DefaultODataBatchHandler(), routeName, routePrefix);
			//}

			var pathHandler = new LoggingPathHandler(this.logger);
			var routeConstraint = new ODataPathRouteConstraint(pathHandler, this.oDataRepository.GetModel(), routeName, routingConventions);
			var odataRoute = new ODataRoute(routePrefix, routeConstraint);

			routes.Add(routeName, odataRoute);
			return odataRoute;
		}

		private static void MapHttpBatchRoute(HttpRouteCollection routes, ODataBatchHandler batchHandler, string routeName, string routePrefix)
		{
			batchHandler.ODataRouteName = routeName;
			var batchTemplate = string.IsNullOrEmpty(routePrefix) ? ODataRouteConstants.Batch : routePrefix + '/' + ODataRouteConstants.Batch;

			routes.MapHttpBatchRoute(routeName + "Batch", batchTemplate, batchHandler);
		}

		private static string RemoveTrailingSlash(string routePrefix)
		{
			if (!string.IsNullOrEmpty(routePrefix) && routePrefix.EndsWith("/"))
			{
				return routePrefix.Substring(0, routePrefix.Length - 1);
			}

			return routePrefix;
		}
	}
}