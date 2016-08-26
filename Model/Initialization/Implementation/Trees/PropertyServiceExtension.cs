namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;

	public static class PropertyServiceExtension
	{
		public static IEnumerable<IProperty> GetSelectionTreePropertiesByTreeSignature(
			this IPropertyService propertyService,
			ISignature treeSignature)
		{
			foreach (var property in propertyService.AllRelevantProperties)
			{
				ISignature signature;
				if (property.TryGetSelectionTreePropertyTreeSignature(out signature) && signature.Equals(treeSignature))
				{
					yield return property;
				}
			}
		}

		public static IEnumerable<IProperty> GetSelectionTreeValueListPropertiesByTreeSignature(
			this IPropertyService propertyService,
			ISignature treeSignature)
		{
			foreach (var property in propertyService.AllRelevantProperties)
			{
				ISignature signature;
				if (property.TryGetSelectionTreeValueListPropertyTreeSignature(out signature) && signature.Equals(treeSignature))
				{
					yield return property;
				}
			}
		}
	}
}
