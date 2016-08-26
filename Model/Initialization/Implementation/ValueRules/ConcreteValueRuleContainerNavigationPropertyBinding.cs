namespace Facton.ServiceApi.Domain.Model.Initialization.ValueRules
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Domain.Dimensions.Metadata;
	using Facton.Domain.Dimensions.Metadata.ValueRules;
	using Facton.Domain.Dimensions.ValueRules;
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Entities.Collections;
	using Facton.Infrastructure.Entities.Deleting;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// Binding for the concrete value rule container of a value rule property.
	/// </summary>
	public class ConcreteValueRuleContainerNavigationPropertyBinding : IContainedCollectionNavigationPropertyBinding
	{
		private readonly IValueRuleService valueRuleService;

		private readonly IValueRuleMetadataService valueRuleMetadataService;

		private readonly IProperty valueProperty;

		public ConcreteValueRuleContainerNavigationPropertyBinding(
			IValueRuleService valueRuleService,
			IValueRuleMetadataService valueRuleMetadataService,
			IProperty valueProperty)
		{
			this.valueRuleService = valueRuleService;
			this.valueRuleMetadataService = valueRuleMetadataService;
			this.valueProperty = valueProperty;
		}


		public IEnumerable<IEntity> GetAll(IEntity parentEntity)
		{
			return this.GetValueRules(parentEntity);
		}

		public bool TryGetByKeys(IEntity parentEntity, IEnumerable<KeyValuePair<string, object>> keys, out IEntity entity)
		{
			var entityId = keys.First().Value.As<IId>();
			entity = this.GetValueRules(parentEntity).SingleOrDefault(valueRule => valueRule.Id.Equals(entityId));
			return entity != null;
		}

		public IEntity CreateAndAdd(IEntity parentEntity, IDictionary<string, IDependency> dependencies)
		{
			var valueRuleContainer =
				this.valueRuleService.GetOrCreateValueRuleContainer(parentEntity, this.valueProperty).As<ICreatingEntityCollection>();

			var itemSpecification = GetItemSpecification(dependencies);
			if (valueRuleContainer.CanCreateAndAdd(itemSpecification))
			{
				return valueRuleContainer.CreateAndAdd(itemSpecification).Item;
			}

			throw new InvalidOperationException("Could not create value rule.");
		}

		public void Delete(IEntity parentEntity, IEntity entityToDelete)
		{
			var deleteableEntity = entityToDelete.As<IDeletable>();
			if (deleteableEntity.CanDelete())
			{
				deleteableEntity.Delete();
			}

			throw new InvalidOperationException("Could not delete value rule.");
		}

		private IEnumerable<IEntity> GetValueRules(IEntity parentEntity)
		{
			IValueRuleSignature valueRuleSignature;
			if (this.valueRuleMetadataService.TryGetValueRuleSignature(parentEntity.Signature, this.valueProperty, out valueRuleSignature))
			{
				ICreatingEntityCollection container;
				if (this.valueRuleService.TryGetExistingContainer(parentEntity, valueRuleSignature.LeadingValueProperty, out container))
				{
					return container;
				}
			}

			return Enumerable.Empty<IEntity>();
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
