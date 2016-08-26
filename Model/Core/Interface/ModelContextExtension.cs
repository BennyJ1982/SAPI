namespace Facton.ServiceApi.Domain.Model.Core
{
	using System.Collections.Generic;

	public static class ModelContextExtension
	{
		public static IEntityType GetEntityTypeOrThrow(this IModelContext context, string name)
		{
			IEntityType entityType;
			if (context.TryGetEntityType(name, out entityType))
			{
				return entityType;
			}

			throw new KeyNotFoundException("Entity type not found: " + name);
		}

		public static IEntitySet GetEntitySetOrThrow(this IModelContext context, string name)
		{
			IEntitySet entitySet;
			if (context.TryGetEntitySet(name, out entitySet))
			{
				return entitySet;
			}

			throw new KeyNotFoundException("Entity set not found: " + name);
		}
	}
}
