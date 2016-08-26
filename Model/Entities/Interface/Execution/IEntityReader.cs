namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;

	using Microsoft.OData.Core.UriParser.Semantic;

	/// <summary>
	/// Reads entities from an odata path. Uses the bindings of the affected entity sets and navigation properties to perform the read.
	/// </summary>
	public interface IEntityReader
	{
		IEnumerable<IEntity> ReadEntitiesFromPath(IBindableModelContext modelContext, IEnumerable<ODataPathSegment> oDataPath);

		IEnumerable<IEntity> ReadEntitiesFromRelativePath(
			IBindableModelContext modelContext,
			IEntity parentEntity,
			IEnumerable<ODataPathSegment> relativeODataPath);
	}
}
