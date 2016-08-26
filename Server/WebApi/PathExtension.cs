namespace ServiceApi.Server.WebApi
{
	using System.Web.OData.Routing;
	using Microsoft.OData.Edm;

	public static class PathExtension
	{
		public static bool ReturnsCollection(this ODataPath odataPath)
		{
			return odataPath.EdmType is IEdmCollectionType;
		}
	}
}
