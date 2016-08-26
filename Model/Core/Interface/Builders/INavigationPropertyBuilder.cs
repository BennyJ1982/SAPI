namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using Microsoft.OData.Edm.Library;

	public interface INavigationPropertyBuilder : INavigatableElementBuilder
	{
		INavigationProperty BuiltNavigationProperty { get; }

		string SourcePropertyName { get; }

		IEntityTypeBuilder TargetEntityTypeBuilder { get; }

		bool IsCollection { get; }

		INavigationProperty Build(EdmEntityType sourceEdmEntityType);
	}
}
