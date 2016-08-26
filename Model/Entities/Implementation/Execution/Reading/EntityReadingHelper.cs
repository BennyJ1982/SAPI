namespace Facton.ServiceApi.Domain.Model.Entities.Execution.Reading
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	using Microsoft.OData.Edm;

	public static class EntityReadingHelper
	{
		public static void ReadSingleton(ReadContext readContext, IEntitySet singleton)
		{
			ISingletonBinding binding;
			if (!readContext.Model.TryGetBinding(singleton, out binding))
			{
				throw new ArgumentException("The specified singleton does not support the expected binding.", nameof(singleton));
			}

			IEntity entity;
			readContext.SetResult(binding.TryGet(out entity) ? entity.Enumerate() : ReadContext.EmptyResult);
		}

		public static void ReadEntitySet(ReadContext readContext, IEntitySet entitySet, IEnumerable<KeyValuePair<string, object>> keys)
		{
			IEntitySetBinding binding;
			if (!readContext.Model.TryGetBinding(entitySet, out binding))
			{
				throw new ArgumentException("The specified entity set does not support the expected binding.", nameof(entitySet));
			}

			IEntity entity;
			readContext.SetResult(binding.TryGetByKeys(keys, out entity) ? entity.Enumerate() : ReadContext.EmptyResult);
		}

		public static void ReadEntitySet(ReadContext readContext, IEntitySet entitySet)
		{
			IEntitySetBinding binding;
			if (!readContext.Model.TryGetBinding(entitySet, out binding))
			{
				throw new ArgumentException("The specified entity set does not support the expected binding.", nameof(entitySet));
			}

			readContext.SetResult(binding.GetAll());
		}

		public static void ReadCollectionNavigationProperty(
			ReadContext readContext,
			INavigationProperty navigationProperty,
			IEnumerable<KeyValuePair<string, object>> keys)
		{
			ICollectionNavigationPropertyBinding binding;
			if (!readContext.Model.TryGetBinding(navigationProperty, out binding))
			{
				throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
			}

			IEntity parentEntity;
			if (TryGetParentEntity(readContext, out parentEntity))
			{
				IEntity entity;
				readContext.SetResult(binding.TryGetByKeys(parentEntity, keys, out entity) ? entity.Enumerate() : ReadContext.EmptyResult);
			}
		}

		public static void ReadCollectionNavigationProperty(ReadContext readContext, INavigationProperty navigationProperty)
		{
			ICollectionNavigationPropertyBinding binding;
			if (!readContext.Model.TryGetBinding(navigationProperty, out binding))
			{
				throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
			}

			IEntity parentEntity;
			if (TryGetParentEntity(readContext, out parentEntity))
			{
				readContext.SetResult(binding.GetAll(parentEntity));
			}
		}

		public static void ReadSingleNavigationProperty(ReadContext readContext, INavigationProperty navigationProperty)
		{
			var multiplicity = navigationProperty.ResultingEdmType.TargetMultiplicity();
			if (!(multiplicity == EdmMultiplicity.One || multiplicity == EdmMultiplicity.ZeroOrOne))
			{
				throw new ArgumentException("The specified navigation property is not a single-value property.", nameof(navigationProperty));
			}

			ISingleNavigationPropertyBinding binding;
			if (!readContext.Model.TryGetBinding(navigationProperty, out binding))
			{
				throw new ArgumentException("The specified navigation property does not support the expected binding.", nameof(navigationProperty));
			}

			IEntity parentEntity;
			if (TryGetParentEntity(readContext, out parentEntity))
			{
				IEntity entity;
				readContext.SetResult(binding.TryGet(parentEntity, out entity) ? entity.Enumerate() : ReadContext.EmptyResult);
			}
		}

		private static bool TryGetParentEntity(ReadContext readContext, out IEntity parentEntity)
		{
			parentEntity = readContext.Result.SingleOrDefault();
			return parentEntity != null;
		}
	}
}
