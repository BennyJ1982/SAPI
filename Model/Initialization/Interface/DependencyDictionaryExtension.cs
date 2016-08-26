namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Domain.Model.Entities;

	public static class DependencyDictionaryExtension
	{
		public static bool TryGetEntityId(this IDictionary<string, IDependency> dependencies, out IId entityId)
		{
			IDependency idDependency;
			if (dependencies.TryGetValue(FactonModelKeywords.IdPropertyName, out idDependency))
			{
				entityId = idDependency.Value as IId;
				return entityId != null;

			}

			entityId = null;
			return false;
		}
	}
}
