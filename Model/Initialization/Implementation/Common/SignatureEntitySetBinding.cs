namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;
	using Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes;

	/// <summary>
	/// General Binding for signature based entity sets.
	/// </summary>
	public class SignatureEntitySetBinding : IEntitySetBinding
	{
		private readonly IFactonQueryService queryService;

		private readonly IQueryBuilderFactory queryBuilderFactory;

		private readonly SignatureQueryAttribute signatureAttribute;

		private readonly EntityTypesQueryAttribute entityTypesAttribute;

		private readonly SpaceQueryAttribute spaceAttribute;

		public SignatureEntitySetBinding(
			IFactonQueryService queryService,
			IQueryBuilderFactory queryBuilderFactory,
			ISignature signature,
			IEnumerable<string> entityTypes)
		{
			this.queryService = queryService;
			this.queryBuilderFactory = queryBuilderFactory;
			this.signatureAttribute = new SignatureQueryAttribute(signature);
			this.entityTypesAttribute = new EntityTypesQueryAttribute(entityTypes.ToArray());
		}

		public SignatureEntitySetBinding(
			IFactonQueryService queryService,
			IQueryBuilderFactory queryBuilderFactory,
			ISignature signature,
			IEnumerable<string> entityTypes,
			string space)
		{
			this.queryService = queryService;
			this.queryBuilderFactory = queryBuilderFactory;
			this.signatureAttribute = new SignatureQueryAttribute(signature);
			this.entityTypesAttribute = new EntityTypesQueryAttribute(entityTypes.ToArray());
			this.spaceAttribute = new SpaceQueryAttribute(space);
		}

		public IEnumerable<IEntity> GetAll()
		{
			var queryBuilder = this.GetQueryBuilder();
			return this.queryService.ExecuteFqlQuery(queryBuilder.BuildQuery());
		}

		public bool TryGetByKeys(IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity)
		{
			var queryBuilder = this.GetQueryBuilder();
			FilterByEntityId(queryBuilder, keys);

			entity = this.queryService.ExecuteFqlQuery(queryBuilder.BuildQuery()).SingleOrDefault();
			return entity != null;
		}

		public IEntity CreateAndAdd(IDictionary<string, IDependency> dependencies)
		{
			throw new System.NotSupportedException("Cannot modify signature set without specific resource type knowledge.");
		}

		public void Delete(IEntity parentEntity, IEntity entityToDelete)
		{
			throw new System.NotSupportedException("Cannot modify signature set without specific resource type knowledge.");
		}

		private IQueryBuilder GetQueryBuilder()
		{
			var builder = this.queryBuilderFactory.Create();

			ApplySelection(builder);
			this.FilterByEntityType(builder);
			this.FilterBySpace(builder);
			this.FilterBySignature(builder);

			return builder;
		}

		private void FilterBySpace(IQueryBuilder builder)
		{
			if (this.spaceAttribute != null)
			{
				builder.FromBuilder.AppendWithComma(this.spaceAttribute.FqlQueryTextFragment);
				builder.Attributes.Add(this.spaceAttribute);
			}
		}

		private void FilterBySignature(IQueryBuilder builder)
		{
			builder.FromBuilder.AppendWithComma(this.signatureAttribute.FqlQueryTextFragment);
			builder.Attributes.Add(this.signatureAttribute);
		}

		private void FilterByEntityType(IQueryBuilder builder)
		{
			builder.FromBuilder.AppendWithComma(this.entityTypesAttribute.FqlQueryTextFragment);
			builder.Attributes.Add(this.entityTypesAttribute);
		}

		private static void FilterByEntityId(IQueryBuilder builder, IEnumerable<KeyValuePair<string, object>> keys)
		{
			var entityIdAttribute = new EntityIdQueryAttribute(keys.First().Value.As<IId>());
			builder.WhereBuilder.Append(entityIdAttribute.FqlQueryTextFragment);
			builder.Attributes.Add(entityIdAttribute);
		}

		private static void ApplySelection(IQueryBuilder builder)
		{
			builder.SelectBuilder.Append("*"); // TODO select properties
		}
	}
}
