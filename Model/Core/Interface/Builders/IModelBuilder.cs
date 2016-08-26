namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using System.Collections.Generic;

	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;

	public interface IModelBuilder
	{
		IEnumerable<IEntityTypeBuilder> EntityTypeBuilders { get; }

		IEnumerable<IEntitySetBuilder> EntitySetBuilders { get; }

		IModelBuilder WithNamespace(string nameSpace);

		IModelBuilder WithContainerName(string containerName);

		IModelBuilder WithComplexDataType(IComplexDataType complexDataType);

		IModelBuilder WithOperationHandler(INavigatableElementBuilder navigatableElementBuilder, IOperationHandler operationHandler);

		IModelBuilder WithOperationHandler(IOperationHandler operationHandler);

		IEntityTypeBuilder CreateEntityTypeBuilder(string name);

		IEntitySetBuilder CreateEntitySetBuilder(string name, IEntityTypeBuilder containedEntityTypeBuilder);

		bool TryGetEntityTypeBuilder(string name, out IEntityTypeBuilder entityTypeBuilder);

		bool TryGetEntitySetBuilder(string name, out IEntitySetBuilder entitySetBuilder);

		IModelContext Build();
	}
}