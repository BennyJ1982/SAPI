namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;

	public interface IEntitySetBuilder : INavigatableElementBuilder
	{
		bool IsSingleton { get; }

		IEntitySet BuiltEntitySet { get; }

		IEntityTypeBuilder ContainedEntityType { get; }

		IEntitySetBuilder AsSingleton();

		IEntitySetBuilder WithSupportedOperations(Operation operation);

		IEntitySetBuilder WithUncontainedNavigationPropertyTarget(
			IUncontainedNavigationPropertyBuilder navigationProperty,
			IEntitySetBuilder targetBuilder);

		IEntitySet Build();

		void BuildUncontainedNavigationPropertyTargets();
	}
}
