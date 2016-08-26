namespace Facton.ServiceApi.Domain.Model.Initialization.Versions
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// The binding for the versioned entity navigation property of a version
	/// </summary>
	public class VersionedEntityNavigationPropertyBinding : IContainedNavigationPropertyBinding
	{
		public bool TryGet(IEntity parentEntity, out IEntity entity)
		{
			// TODO: var version = parentEntity.As<IVersion>();
			entity = null;
			return false;
		}

		public IEntity CreateAndSet(IEntity parentEntity, IDictionary<string, IDependency> dependencies)
		{
			throw new NotSupportedException("Cannot set versioned entity as it is part of the version.");
		}

		public void Delete(IEntity parentEntity)
		{
			throw new NotSupportedException("Cannot delete versioned entity as it is part of the version.");
		}
	}
}



///// <summary>
///// Data source provider for the versioned entity navigation property of a version info
///// </summary>
//public class VersionedEntityQueryProvider : IQueryProvider
//{
//	private const string VersionNumberPropertyName = "_VersionNumber";

//	private readonly IQueryBuilderFactory queryBuilderFactory;
//	private readonly IMetadataService metadataService;

//	public VersionedEntityQueryProvider(IQueryBuilderFactory queryBuilderFactory, IMetadataService metadataService)
//	{
//		this.queryBuilderFactory = queryBuilderFactory;
//		this.metadataService = metadataService;
//	}

//	public IQueryBuilder ProvideQuery(IOperationContext operationContext)
//	{
//		var versionNumber = this.GetVersionNumber(operationContext.Parent);
//		var builder = this.queryBuilderFactory.Create();

//		// TODO
//		builder.FromBuilder.Append(", HISTORY versioned");
//		builder.WhereBuilder.AppendWithAnd($"{VersionNumberPropertyName}={versionNumber}");
//		return builder;
//	}

//	/// <summary>
//	/// Executes the data source of the specified operation context and extracts the version number from the first entity it returns.
//	/// </summary>
//	private long GetVersionNumber(IOperationContext operationContext)
//	{
//		var dataSource = operationContext.GetDataSource();
//		var columns = new[] { VersionNumberPropertyName };
//		return dataSource.GetData(columns).Select(this.ExtractVersionNumberFromEntity).Single();
//	}

//	private long ExtractVersionNumberFromEntity(IEntity entity)
//	{
//		var versionNumberProperty = this.metadataService.GetPropertyByName(VersionNumberPropertyName);
//		return entity.GetValue(versionNumberProperty).As<IntegerValue>().Value;
//	}

//}
