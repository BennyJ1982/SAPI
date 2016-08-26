namespace Facton.ServiceApi.Domain.Model.Entities.Execution.Reading
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Entities;

	using Microsoft.OData.Core.UriParser.Semantic;

	public class EntityReader : IEntityReader
	{
		public IEnumerable<IEntity> ReadEntitiesFromPath(IBindableModelContext modelContext, IEnumerable<ODataPathSegment> oDataPath)
		{
			if (!oDataPath.Any())
			{
				throw new ArgumentException("The path must not be empty.", nameof(oDataPath));
			}

			var readingHandler = new EntityReadingPathSegmentHandler(modelContext);
			readingHandler.HandlePath(oDataPath);
			return readingHandler.GetResultingEntities();
		}

		public IEnumerable<IEntity> ReadEntitiesFromRelativePath(
			IBindableModelContext modelContext,
			IEntity parentEntity,
			IEnumerable<ODataPathSegment> relativeODataPath)
		{
			if (!relativeODataPath.Any())
			{
				throw new ArgumentException("The path must not be empty.", nameof(relativeODataPath));
			}

			var readingHandler = new EntityReadingPathSegmentHandler(modelContext, parentEntity);
			readingHandler.HandlePath(relativeODataPath);
			return readingHandler.GetResultingEntities();
		}
	}
}
