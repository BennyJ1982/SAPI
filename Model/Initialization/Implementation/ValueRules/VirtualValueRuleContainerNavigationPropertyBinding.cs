namespace Facton.ServiceApi.Domain.Model.Initialization.ValueRules
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// Binding for the virtual value rule container navigation property, containing all concrete value container navigation properties
	/// </summary>
	public class VirtualValueRuleContainerNavigationPropertyBinding : IContainedNavigationPropertyBinding
	{
		public bool TryGet(IEntity parentEntity, out IEntity entity)
		{
			entity = parentEntity;
			return true;
		}

		public IEntity CreateAndSet(IEntity parentEntity, IDictionary<string, IDependency> dependencies)
		{
			return parentEntity;
		}

		public void Delete(IEntity parentEntity)
		{
			throw new NotSupportedException("The virtual value rule container cannot be deleted as it is always just there.");
		}
	}
}
