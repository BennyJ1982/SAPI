namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Metadata;

	public interface IEntityTypeBuilder
	{
		bool AlreadyBuilt { get; }

		bool NavigationPropertiesAlreadyBuilt { get; }

		IEntityType BuiltEntityType { get; }

		IEnumerable<IStructuralProperty> KeyProperties { get;}

		IEnumerable<IStructuralProperty> Properties { get; }

		IEntityTypeBuilder ParentTypeBuilder { get; }

		IEntityTypeBuilder AsOpenType();

		IEntityTypeBuilder AsAbstractType();

		IEntityTypeBuilder WithParentType(IEntityTypeBuilder parentTypeBuilder);

		IEntityTypeBuilder WithKeyProperty(IStructuralProperty property);

		IEntityTypeBuilder WithStructuralProperty(IStructuralProperty property);

		IEnumerable<INavigationPropertyBuilder> NavigationPropertyBuilders { get; }

		IContainedNavigationPropertyBuilder CreateContainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, string propertyName);

		IContainedNavigationPropertyBuilder CreateContainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, IProperty propertyName);

		IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, string propertyName);

		IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationProperty(IEntityTypeBuilder targetTypeBuilder, IProperty propertyName);

		IEntityType Build();

		void BuildNavigationProperties();
	}
}
