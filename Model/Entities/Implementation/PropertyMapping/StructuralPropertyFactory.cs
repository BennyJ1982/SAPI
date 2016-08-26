namespace Facton.ServiceApi.Domain.Model.Entities.PropertyMapping
{
	using System;

	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	public class StructuralPropertyFactory : IStructuralPropertyFactory
	{
		private readonly IDataTypeLookup dataTypes;

		private readonly IModelItemNamingService modelItemNamingService;

		public StructuralPropertyFactory(IDataTypeLookup dataTypes, IModelItemNamingService modelItemNamingService)
		{
			this.dataTypes = dataTypes;
			this.modelItemNamingService = modelItemNamingService;
		}

		public bool TryCreate(IProperty property, out IStructuralProperty structuralProperty)
		{
			IDataType dataType;
			if (!this.TryGetDataType(property, out dataType))
			{
				structuralProperty = null;
				return false;
			}

			structuralProperty = new CostSharpStructuralProperty(property, this.modelItemNamingService.GetSafeEdmPropertyName(property), dataType);
			return true;
		}

		public bool TryCreate(IProperty property, string edmPropertyName, out IStructuralProperty structuralProperty)
		{
			IDataType dataType;
			if (!this.TryGetDataType(property, out dataType))
			{
				structuralProperty = null;
				return false;
			}

			structuralProperty = new CostSharpStructuralProperty(property, edmPropertyName, dataType);
			return true;
		}

		public bool TryCreate(IProperty property, string edmPropertyName, bool canBeNull, out IStructuralProperty structuralProperty)
		{
			IDataType dataType;
			if (!this.TryGetDataType(property, out dataType))
			{
				structuralProperty = null;
				return false;
			}

			structuralProperty = new CostSharpStructuralProperty(property, edmPropertyName, dataType, canBeNull);
			return true;
		}

		public bool TryCreate<T>(string edmPropertyName, Func<IEntity, T> valueGetter, bool canBeNull, out IStructuralProperty structuralProperty)
		{
			IValueBasedDataType dataType;
			if (!this.TryGetValueBasedDataType<T>(out dataType))
			{
				structuralProperty = null;
				return false;
			}

			structuralProperty = new StructuralProperty<T>(edmPropertyName, dataType, valueGetter, canBeNull);
			return true;
		}

		public bool TryCreate<T>(
			string edmPropertyName,
			Func<IEntity, T> valueGetter,
			Action<IEntity, T> valueSetter,
			bool canBeNull,
			out IStructuralProperty structuralProperty)
		{
			IValueBasedDataType dataType;
			if (!this.TryGetValueBasedDataType<T>(out dataType))
			{
				structuralProperty = null;
				return false;
			}

			structuralProperty = new StructuralProperty<T>(edmPropertyName, dataType, valueGetter, valueSetter, canBeNull);
			return true;
		}

		private bool TryGetDataType(IProperty property, out IDataType dataType)
		{
			dataType = this.dataTypes.GetByKey(property.DomainType);
			return dataType != null;
		}

		private bool TryGetDataType<T>(out IDataType dataType)
		{
			dataType = this.dataTypes.GetByKey(typeof(T));
			return dataType != null;
		}

		private bool TryGetValueBasedDataType<T>(out IValueBasedDataType valueBasedDataType)
		{
			IDataType dataType;
			if (!this.TryGetDataType<T>(out dataType))
			{
				valueBasedDataType = null;
				return false;
			}

			valueBasedDataType = dataType as IValueBasedDataType;
			return valueBasedDataType != null;
		}
	}
}
