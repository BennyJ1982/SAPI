namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;

	public static class EntityExtension
	{
		public static bool TryGetSignificantValue<T>(this IEntity entity, IProperty property, out T value) where T : class
		{
			T castValue;
			if (entity.TryGetValue(property, out castValue) && !castValue.Equals(property.DomainType.EmptyValue))
			{
				value = castValue;
				return true;
			}

			value = null;
			return false;
		}

		public static bool TryGetValue<T>(this IEntity entity, IProperty property, out T value) where T : class
		{
			var objectValue = entity.GetValue(property);
			var castValue = objectValue as T;
			if (castValue != null)
			{
				value = castValue;
				return true;
			}

			value = null;
			return false;
		}
	}
}
