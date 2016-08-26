namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;
	using Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes;

	/// <summary>
	/// Binding for singletons representing a FACTON signature.
	/// </summary>
	public class SignatureSingletonBinding : ISingletonBinding
	{
		private readonly IFactonQueryService queryService;

		private readonly IQueryBuilderFactory queryBuilderFactory;

		private readonly SignatureQueryAttribute signatureAttribute;

		private readonly EntityTypesQueryAttribute entityTypesAttribute;

		private readonly SpaceQueryAttribute spaceAttribute;

		public SignatureSingletonBinding(
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

		public bool TryGet(out IEntity entity)
		{
			var queryBuilder = this.GetQueryBuilder();
			entity = this.queryService.ExecuteFqlQuery(queryBuilder.BuildQuery()).SingleOrDefault();
			return entity != null;
		}

		private IQueryBuilder GetQueryBuilder()
		{
			var builder = this.queryBuilderFactory.Create();
			ApplySelection(builder);

			builder.FromBuilder.AppendWithComma(this.entityTypesAttribute.FqlQueryTextFragment);
			builder.FromBuilder.AppendWithComma(this.spaceAttribute.FqlQueryTextFragment);
			builder.FromBuilder.AppendWithComma(this.signatureAttribute.FqlQueryTextFragment);

			builder.Attributes.Add(this.entityTypesAttribute);
			builder.Attributes.Add(this.spaceAttribute);
			builder.Attributes.Add(this.signatureAttribute);

			return builder;
		}

		private static void ApplySelection(IQueryBuilder builder)
		{
			builder.SelectBuilder.Append("*"); // TODO select properties
		}
	}
}
