namespace Facton.ServiceApi.Domain.Model.Core
{
	using System.Collections.Generic;

	using Microsoft.OData.Edm;

	public interface IEntityType 
	{
		string Name { get; }

		IEdmEntityType ResultingEdmEntityType { get; }

		IEnumerable<IStructuralProperty> StructuralProperties { get; }

		IEnumerable<IStructuralProperty> KeyProperties { get; }

		bool TryGetStructuralProperty(string propertyName, out IStructuralProperty property);

		bool TryGetKeyProperty(string propertyName, out IStructuralProperty keyProperty);

		bool TryGetNavigationProperty(string sourcePropertyName, out INavigationProperty navigationProperty);
	}
}
