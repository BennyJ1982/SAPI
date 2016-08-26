namespace Facton.ServiceApi.Domain.Model.Entities.PropertyMapping
{
	using System;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	public class StructuralProperty<T> : IStructuralEntityProperty
	{
		private readonly Func<IEntity, T> valueGetter;

		private readonly Action<IEntity, T> valueSetter;

		private readonly IValueBasedDataType dataType;

		public StructuralProperty(string edmPropertyName, IValueBasedDataType dataType, Func<IEntity, T> valueGetter, bool canBeNull)
			: this(edmPropertyName, dataType, valueGetter, null, canBeNull)
		{
		}

		public StructuralProperty(
			string edmPropertyName,
			IValueBasedDataType dataType,
			Func<IEntity, T> valueGetter,
			Action<IEntity, T> valueSetter,
			bool canBeNull)
		{
			this.EdmPropertyName = edmPropertyName;
			this.dataType = dataType;
			this.valueGetter = valueGetter;
			this.valueSetter = valueSetter;
			this.CanBeNull = canBeNull;
		}

		public string EdmPropertyName { get; }

		public bool CanBeNull { get; }

		public IDataType DataType => this.dataType;

		public object Serialize(object value)
		{
			return this.dataType.Serialize(value);
		}

		public object Deserialize(object odataValue)
		{
			return this.dataType.Deserialize(odataValue);
		}

		public bool TryGetValueFromEntity(IEntity entity, out object value)
		{
			value = this.GetValue(entity);
			return true;
		}

		public bool TrySetValueOnEntity(IEntity entity, object value)
		{
			if (!this.IsReadOnly(entity))
			{
				this.SetValue(entity, (T)value);
				return true;
			}

			return false;
		}

		public bool IsReadOnly(IEntity targetEntity)
		{
			return this.valueSetter == null;
		}

		private T GetValue(IEntity entity)
		{
			return this.valueGetter(entity);
		}

		private void SetValue(IEntity entity, T value)
		{
			if (this.valueSetter == null)
			{
				throw new NotSupportedException("Property " + this.EdmPropertyName + " is read-only.");
			}

			this.valueSetter(entity, value);
		}
	}
}
