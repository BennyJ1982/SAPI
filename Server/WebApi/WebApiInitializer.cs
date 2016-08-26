namespace ServiceApi.Server.WebApi
{
	using System.Web.Http;
	using System.Web.Http.Dispatcher;
	using System.Web.OData.Extensions;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Core.UriParser.Semantic;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Repository;
	using ServiceApi.Server.WebApi.Binding;
	using ServiceApi.Server.WebApi.Controllers;
	using ServiceApi.Server.WebApi.ODataObjects;
	using ServiceApi.Server.WebApi.Routing;

	internal static class WebApiInitializer
	{
		public static HttpConfiguration Configure(IODataRepository oDataRepository, IMappingLogger logger)
		{
			var config = new HttpConfiguration();
			var routeFactory = new RouteFactory(oDataRepository, logger);
			var controllerActivator = new ControllerActivator(
				oDataRepository,
				config.Services.GetService(typeof(IHttpControllerActivator)).As<IHttpControllerActivator>());

			ConfigureServices(config, controllerActivator);
			ConfigureRoutes(config, routeFactory);
			ConfigureBindings(config);

			config.AddODataQueryFilter();
			return config;
		}

		private static void ConfigureServices(HttpConfiguration config, IHttpControllerActivator controllerActivator)
		{
			config.Services.Replace(typeof(IHttpControllerActivator), controllerActivator);
		}

		private static void ConfigureRoutes(HttpConfiguration config, IRouteFactory routeFactory)
		{
			// odata route
			routeFactory.MapCustomODataServiceRoute(config.Routes, "serviceapi", "serviceapi")
				.HasRelaxedODataVersionConstraint(); // allow any odata version headers

			// "home page" listing all data sources
			config.Routes.MapHttpRoute("home", "{controller}", new { controller = "Home" });
		}

		private static void ConfigureBindings(HttpConfiguration config)
		{
			config.ParameterBindingRules.Add(p => p.ParameterType == typeof(ODataPath) ? new ODataPathParameterBinding(p) : null);
			config.ParameterBindingRules.Add(p => p.ParameterType == typeof(QueryOptions) ? new ODataQueryOptionsParameterBinding(p) : null);
			config.ParameterBindingRules.Add(p => p.ParameterType == typeof(ServiceApiEdmEntityObject) ? new NonValidatingParameterBinding(p) : null);
		}
	}
}