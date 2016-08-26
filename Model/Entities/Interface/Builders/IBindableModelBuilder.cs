namespace Facton.ServiceApi.Domain.Model.Entities.Builders
{
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// A model builder which supports bindings.
	/// </summary>
	public interface IBindableModelBuilder : IModelBuilder
	{
		IBindableModelBuilder WithDependency(INavigatableElementBuilder navigatableElementBuilder, IStructuralProperty dependency);

		IBindableModelBuilder WithDependency(INavigatableElementBuilder navigatableElementBuilder, INavigationPropertyBuilder dependency);

		IBindableModelBuilder WithOptionalDependency(INavigatableElementBuilder navigatableElementBuilder, IStructuralProperty dependency);

		IBindableModelBuilder WithOptionalDependency(INavigatableElementBuilder navigatableElementBuilder, INavigationPropertyBuilder dependency);

		IBindableModelBuilder WithBinding(INavigatableElementBuilder navigatableElementBuilder, IBinding binding);

		bool TryGetBinding<T>(INavigatableElementBuilder navigatableElementBuilder, out T binding) where T : class, IBinding;
	}
}
