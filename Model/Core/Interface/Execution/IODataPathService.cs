namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using System.Collections.Generic;

	using Microsoft.OData.Core.UriParser.Semantic;

	public interface IODataPathService
	{
		INavigatable GetNavigationTarget(IModelContext modelContext , IEnumerable<ODataPathSegment> oDataPath);

		IEntitySet GetNavigationRoot(IModelContext modelContext, IEnumerable<ODataPathSegment> oDataPath);
	}
}
