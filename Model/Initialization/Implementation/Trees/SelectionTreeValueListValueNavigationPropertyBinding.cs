namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Metadata.DomainValues;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	public class SelectionTreeValueListValueNavigationPropertyBinding : IUncontainedCollectionNavigationPropertyBinding
	{
		private readonly IProperty selectionTreeValueListValueProperty;

		public SelectionTreeValueListValueNavigationPropertyBinding(IProperty selectionTreeValueListValueProperty)
		{
			this.selectionTreeValueListValueProperty = selectionTreeValueListValueProperty;
		}

		public IEnumerable<IEntity> GetAll(IEntity parentEntity)
		{
			return this.GetSelectionTreeValues(parentEntity).Select(s => s.TreeNode.As<IEntity>());
		}

		public bool TryGetByKeys(IEntity parentEntity, IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity)
		{
			var guid = keys.First().Value.As<IId>().GetInternalGuid();
			entity = this.GetSelectionTreeValues(parentEntity)
				.Where(v => v.TreeNodeId.Equals(guid)).Select(s => s.TreeNode.As<IEntity>()).SingleOrDefault();
			return entity != null;
		}

		public void Add(IEntity parentEntity, IEntity entityToAdd)
		{
			if (parentEntity.Expects(this.selectionTreeValueListValueProperty))
			{
				var guids = this.GetSelectionTreeValues(parentEntity).Select(v => v.TreeNodeId).ToHashSet();
				guids.Add(entityToAdd.Id.GetInternalGuid());

				if (this.TrySetSelectiontreeValues(parentEntity, guids))
				{
					return;
				}
			}

			throw new InvalidOperationException("Could not change property " + this.selectionTreeValueListValueProperty.Name);
		}

		public void Remove(IEntity parentEntity, IEntity entityToRemove)
		{
			if (parentEntity.Expects(this.selectionTreeValueListValueProperty))
			{
				var guids = this.GetSelectionTreeValues(parentEntity).Select(v => v.TreeNodeId).ToHashSet();
				guids.Remove(entityToRemove.Id.GetInternalGuid());

				if (this.TrySetSelectiontreeValues(parentEntity, guids))
				{
					return;
				}
			}

			throw new InvalidOperationException("Could not change property " + this.selectionTreeValueListValueProperty.Name);
		}

		private IEnumerable<ISelectionTreeValue> GetSelectionTreeValues(IEntity parentEntity)
		{
			if (parentEntity.Provides(this.selectionTreeValueListValueProperty))
			{
				ISelectionTreeValueListValue value;
				if (parentEntity.TryGetSignificantValue(this.selectionTreeValueListValueProperty, out value))
				{
					return value.SortedSelectionTreeValues;
				}
			}

			return Enumerable.Empty<ISelectionTreeValue>();
		}

		private bool TrySetSelectiontreeValues(IEntity parentEntity, IEnumerable<Guid> treeNodeIds)
		{
			object newValue;
			if (this.selectionTreeValueListValueProperty.DomainType.TryParse(treeNodeIds, CultureInfo.CurrentCulture, out newValue))
			{
				if (parentEntity.TrySetValue(this.selectionTreeValueListValueProperty, newValue))
				{
					return true;
				}
			}

			return false;
		}
	}
}
