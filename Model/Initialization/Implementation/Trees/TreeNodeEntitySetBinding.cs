namespace Facton.ServiceApi.Domain.Model.Initialization.Trees
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Entities.Collections;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	public class TreeNodeEntitySetBinding : IEntitySetBinding
	{
		private readonly IEntitySetBinding commonEntitySetBinding;

		public TreeNodeEntitySetBinding(IEntitySetBinding commonEntitySetBinding)
		{
			this.commonEntitySetBinding = commonEntitySetBinding;
		}

		public IEnumerable<IEntity> GetAll()
		{
			return this.commonEntitySetBinding.GetAll();
		}

		public bool TryGetByKeys(IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity)
		{
			return this.commonEntitySetBinding.TryGetByKeys(keys, out entity);
		}

		public IEntity CreateAndAdd(IDictionary<string, IDependency> dependencies)
		{
			var parentNode = GetParentNode(dependencies).As<ICreatingEntityCollection>();
			var itemSpecification = GetItemSpecification(dependencies);

			if (parentNode.CanCreateAndAdd(itemSpecification))
			{
				return parentNode.CreateAndAdd(itemSpecification).Item;
			}

			throw new InvalidOperationException("Could not create tree node.");
		}

		public void Delete(IEntity parentEntity, IEntity entityToDelete)
		{
			throw new NotImplementedException();
		}

		private static IEntity GetParentNode(IDictionary<string, IDependency> dependencies)
		{
			IDependency parentNodeDependency;
			if (dependencies.TryGetValue(FactonModelKeywords.TreeNodeParentNavigationPropertyName, out parentNodeDependency))
			{
				var parentEntity = parentNodeDependency?.Value.As<IEnumerable<IEntity>>().SingleOrDefault();
				if (parentEntity != null)
				{
					return parentEntity;
				}
			}

			throw new InvalidOperationException("Tree node parent not specified or not found.");
		}

		private static IItemSpecification GetItemSpecification(IDictionary<string, IDependency> dependencies)
		{
			IId entityId;
			if (dependencies.TryGetEntityId(out entityId))
			{
				return new DefaultCreateEntityWithIdSpecification(entityId);
			}

			return DefaultCreateEntitySpecification.Instance;
		}
	}
}
