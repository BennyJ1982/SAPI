namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using System;
	using System.Globalization;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Metadata.DomainValues;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	public class SelectionTreeValueNavigationPropertyBinding : IUncontainedNavigationPropertyBinding
	{
		private readonly IProperty selectionTreeValueProperty;

		public SelectionTreeValueNavigationPropertyBinding(IProperty selectionTreeValueProperty)
		{
			this.selectionTreeValueProperty = selectionTreeValueProperty;
		}

		public bool TryGet(IEntity parentEntity, out IEntity entity)
		{
			if (parentEntity.Provides(this.selectionTreeValueProperty))
			{
				ISelectionTreeValue value;
				if (parentEntity.TryGetSignificantValue(this.selectionTreeValueProperty, out value))
				{
					entity = value.TreeNode.As<IEntity>();
					return true;
				}
			}

			entity = null;
			return false;
		}

		public void Set(IEntity parentEntity, IEntity entityToSet)
		{
			var dataTreeProperty = this.selectionTreeValueProperty;
			if (parentEntity.Expects(dataTreeProperty))
			{
				object selectionTreeValue;
				if (dataTreeProperty.DomainType.TryParse(entityToSet.Id.GetInternalGuid(), CultureInfo.CurrentCulture, out selectionTreeValue))
				{
					if (parentEntity.TrySetValue(dataTreeProperty, selectionTreeValue))
					{
						return;
					}
				}
			}

			throw new InvalidOperationException("Could not set property " + this.selectionTreeValueProperty.Name);
		}

		public void Clear(IEntity parentEntity)
		{
			var dataTreeProperty = this.selectionTreeValueProperty;
			if (parentEntity.Expects(dataTreeProperty))
			{
				if (parentEntity.TrySetValue(dataTreeProperty, dataTreeProperty.DomainType.EmptyValue))
				{
					return;
				}

			}

			throw new InvalidOperationException("Could not clear property " + this.selectionTreeValueProperty.Name);
		}
	}
}
