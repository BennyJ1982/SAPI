namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using System;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// Retrieves the parent node of a tree node.
	/// </summary>
	public class TreeNodeParentNavigationPropertyBinding : IUncontainedNavigationPropertyBinding
	{
		public bool TryGet(IEntity parentEntity, out IEntity entity)
		{
			entity = parentEntity.As<ILogicalTreeItem>().Parent;
			return entity != null;
		}

		public void Set(IEntity parentEntity, IEntity entityToSet)
		{
			throw new NotSupportedException("Cannot change parent of a tree node.");
		}

		public void Clear(IEntity parentEntity)
		{
			throw new NotSupportedException("Cannot remove parent of a tree node.");
		}
	}
}
