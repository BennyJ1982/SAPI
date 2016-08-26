namespace Facton.ServiceApi.Domain.Model.Initialization.Versions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;
	using Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes;

	public class VersionInfoNavigationPropertyBinding : IContainedCollectionNavigationPropertyBinding
	{
		private readonly IFactonQueryService queryService;

		private readonly IQueryBuilderFactory queryBuilderFactory;

		public VersionInfoNavigationPropertyBinding(IFactonQueryService queryService, IQueryBuilderFactory queryBuilderFactory)
		{
			this.queryService = queryService;
			this.queryBuilderFactory = queryBuilderFactory;
		}

		public IEnumerable<IEntity> GetAll(IEntity parentEntity)
		{
			var queryBuilder = this.GetQueryBuilder(parentEntity.SpaceId);
			return this.queryService.ExecuteFqlQuery(queryBuilder.BuildQuery());
		}

		public bool TryGetByKeys(IEntity parentEntity, IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity)
		{
			var queryBuilder = this.GetQueryBuilder(parentEntity.SpaceId);
			FilterByEntityId(queryBuilder, keys);
			entity = this.queryService.ExecuteFqlQuery(queryBuilder.BuildQuery()).SingleOrDefault();
			return entity != null;
		}

		public IEntity CreateAndAdd(IEntity parentEntity, IDictionary<string, IDependency> dependencies)
		{
			// TODO BJ: Find out how to create versions now as we only have access to IReadOnlyVersionService.
			throw new NotImplementedException("Versions can't be created");

			// return this.versionService.CreateVersion(parentEntity.Space.RootEntity);
		}

		public void Delete(IEntity parentEntity, IEntity entityToDelete)
		{
			throw new NotImplementedException();
		}

		private IQueryBuilder GetQueryBuilder(IId spaceId)
		{
			var builder = this.queryBuilderFactory.Create();

			builder.FromBuilder.Append($"SPACE \"{spaceId}\", Types version");
			return builder;
		}

		private static void FilterByEntityId(IQueryBuilder builder, IEnumerable<KeyValuePair<string, object>> keys)
		{
			var entityIdAttribute = new EntityIdQueryAttribute(keys.First().Value.As<IId>());
			builder.WhereBuilder.Append(entityIdAttribute.FqlQueryTextFragment);
			builder.Attributes.Add(entityIdAttribute);
		}
	}
}
