namespace ServiceApi.Server.WebApi.Routing
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Net.Http;
	using System.Web.Http.Controllers;
	using System.Web.OData.Routing;
	using System.Web.OData.Routing.Conventions;
	using ServiceApi.Model.Core.Serialization;

	/// <summary>
	/// OData routing conventions which routes all valid requests to the HandleAllController
	/// </summary>
	public class MatchAllRoutingConvention : IODataRoutingConvention
	{
		private readonly IMappingLogger logger;

		public MatchAllRoutingConvention(IMappingLogger logger)
		{
			this.logger = logger;
		}

		public string SelectAction(
			ODataPath odataPath,
			HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
		{
			var requestMethod = controllerContext.Request.Method;
			if (IsValidPath(odataPath))
			{
				if (requestMethod == HttpMethod.Get)
				{
					// it's a path we recognize, so use our special controller actions
					return odataPath.ReturnsCollection() ? "GetEntityCollection" : "GetEntity";
				}

				if (requestMethod == HttpMethod.Post)
				{
					return "PostEntity";
				}

				if (requestMethod.Method == "PATCH")
				{
					return "PatchEntity";
				}
			}
			else
			{
				this.logger.Write(TraceEventType.Error, "Cannot map path " + odataPath);
			}

			// otherwise we just fall back to the default odata routing conventions 
			return null;
		}

		public string SelectController(ODataPath odataPath, HttpRequestMessage request)
		{
			var firstSegment = odataPath.Segments.FirstOrDefault();
			return firstSegment != null && ValidTopLevelPathSegmentKinds.Contains(firstSegment.SegmentKind) ? "HandleAll" : null;
		}

		private static bool IsValidPath(ODataPath odataPath)
		{
			return odataPath.Segments.Count >= 1 
				&& ValidTopLevelPathSegmentKinds.Contains(odataPath.Segments.First().SegmentKind) 
				&& odataPath.Segments.Skip(1).All(
					s => s.SegmentKind == ODataSegmentKinds.Navigation || s.SegmentKind == ODataSegmentKinds.Key);
		}

		private static IEnumerable<string> ValidTopLevelPathSegmentKinds
		{
			get
			{
				yield return ODataSegmentKinds.EntitySet;
				yield return ODataSegmentKinds.Singleton;
			}
		}
	}
}