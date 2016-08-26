// <copyright file="ODataEntityObjectExtension.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Serialization
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;

	public static class ODataEntityObjectExtension
	{
		public static bool TryGetPropertyValue(this ODataEntityDto oDataEntity, IStructuralProperty property, out object value)
		{
			var targetProperty = oDataEntity.Entry.Properties.FirstOrDefault(p => p.Name == property.EdmPropertyName);
			if (targetProperty != null)
			{
				var odataPropertyValue = targetProperty.Value;
				value = property.Deserialize(odataPropertyValue);
				return true;
			}

			value = null;
			return false;
		}

		public static bool TryGetInlineODataEntities(
			this ODataEntityDto oDataEntity,
			string navigationPropertyName,
			out IEnumerable<ODataEntityDto> includedODataEntities)
		{
			var navigationProperty = oDataEntity.NavigationProperties.FirstOrDefault(p => p.Name == navigationPropertyName);
			if (navigationProperty != null)
			{
				var collectionValue = navigationProperty.Value as IEnumerable<ODataEntityDto>;
				if (collectionValue != null)
				{
					includedODataEntities = collectionValue;
					return true;
				}

				var value = navigationProperty.Value as ODataEntityDto;
				if (value != null)
				{
					includedODataEntities = value.Enumerate();
					return true;
				}
			}

			includedODataEntities = null;
			return false;
		}

		public static IEntityType GetEntityType(this ODataEntityDto oDataEntity, IModelContext context)
		{
			var edmEntityType = context.EdmModel.FindDeclaredType(oDataEntity.Entry.TypeName);
			if (edmEntityType != null)
			{
				IEntityType entityType;
				if (context.TryGetEntityType(edmEntityType.Name, out entityType))
				{
					return entityType;
				}

			}

			throw new KeyNotFoundException("The entity type " + oDataEntity.Entry.TypeName + " could not be found.");
		}
	}
}