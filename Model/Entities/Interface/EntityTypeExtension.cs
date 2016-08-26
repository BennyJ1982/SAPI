namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Domain.Model.Core;

	public static class EntityTypeExtension
	{
		public static IEnumerable<IStructuralEntityProperty> GetStructuralAndKeyProperties(this IEntityType entityType)
		{
			return entityType.KeyProperties.Concat(entityType.StructuralProperties).Cast<IStructuralEntityProperty>();
		}

		public static bool TryGetStructuralOrKeyProperty(this IEntityType entityType, string propertyName, out IStructuralEntityProperty property)
		{
			IStructuralProperty foundProperty;
			if (entityType.TryGetStructuralProperty(propertyName, out foundProperty) || entityType.TryGetKeyProperty(propertyName, out foundProperty))
			{
				property = foundProperty.As<IStructuralEntityProperty>();
				return true;
			}

			property = null;
			return false;
		}
	}
}
