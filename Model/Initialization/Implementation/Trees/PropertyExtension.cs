namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using Facton.Infrastructure.Metadata;

	public static class PropertyExtension
	{
		public static bool TryGetTreeSignature(this IProperty property, out ISignature treeSignature)
		{
			return property.TryGetSelectionTreePropertyTreeSignature(out treeSignature)
				|| property.TryGetSelectionTreeValueListPropertyTreeSignature(out treeSignature);
		}

		public static bool TryGetSelectionTreePropertyTreeSignature(this IProperty property, out ISignature treeSignature)
		{
			var selectionTreeDomainType = property.DomainType as ISelectionTreeDomainType;
			if (selectionTreeDomainType != null)
			{
				treeSignature = selectionTreeDomainType.TreeSignature;
				return true;
			}

			treeSignature = null;
			return false;
		}

		public static bool TryGetSelectionTreeValueListPropertyTreeSignature(this IProperty property, out ISignature treeSignature)
		{
			var selectionTreeValueListDomainType = property.DomainType as ISelectionTreeValueListDomainType;
			if (selectionTreeValueListDomainType != null)
			{
				treeSignature = selectionTreeValueListDomainType.ItemDomainType.TreeSignature;
				return true;
			}

			treeSignature = null;
			return false;
		}

	}
}
