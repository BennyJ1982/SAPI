namespace Facton.ServiceApi.Domain.Model.Entities.PropertyMapping
{
	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	public class CostSharpStructuralProperty : IStructuralEntityProperty
	{
		public CostSharpStructuralProperty(IProperty property, string edmPropertyName, IDataType dataType)
			: this(property, edmPropertyName, dataType, true)
		{
		}

		public CostSharpStructuralProperty(IProperty property, string edmPropertyName, IDataType dataType, bool canBeNull)
		{
			this.Property = property;
			this.EdmPropertyName = edmPropertyName;
			this.DataType = dataType;
			this.CanBeNull = canBeNull;
		}

		public string EdmPropertyName { get; }

		public IProperty Property { get; }

		public bool CanBeNull { get; }

		public IDataType DataType { get; }

		public object Serialize(object value)
		{
			return this.DataType.Serialize(value, this.Property.DomainType);
		}

		public object Deserialize(object odataValue)
		{
			return this.DataType.Deserialize(odataValue, this.Property.DomainType);
		}

		public bool TryGetValueFromEntity(IEntity entity, out object value)
		{
			if (!entity.Provides(this.Property))
			{
				value = null;
				return false;
			}

			value = entity.GetValue(this.Property);
			return true;
		}

		public bool TrySetValueOnEntity(IEntity entity, object value)
		{
			if (!this.IsReadOnly(entity))
			{
				var result = entity.TrySetValue(this.Property, value);
				if (result.IsSuccessful)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsReadOnly(IEntity targetEntity)
		{
			return !targetEntity.Expects(this.Property);
		}
	}
}
