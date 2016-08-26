namespace Facton.ServiceApi.Domain.Model.Initialization.DataTypes
{
	using System;
	using System.Globalization;

	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Metadata.DomainValues;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.EdmToClrConversion;
	using Microsoft.OData.Edm.Library;
	using Microsoft.OData.Edm.Values;

	public class MetadataItemDataType : IDomainTypeDependingDataType
	{
		private readonly IMetadataService metadataService;

		private readonly EdmToClrConverter converter = new EdmToClrConverter();

		public MetadataItemDataType(IMetadataService metadataService)
		{
			this.metadataService = metadataService;
		}

		public IEdmTypeReference GetEdmTypeReference(bool nullable = true)
		{
			return EdmCoreModel.Instance.GetPrimitive(EdmPrimitiveTypeKind.String, nullable);
		}

		public object Serialize(object domainValue, IDomainType domainType)
		{
			var metadataItemValue = domainValue as IMetadataItemValue;
			if (metadataItemValue != null)
			{
				return metadataItemValue.MetadataItem.Name;
			}

			throw new ArgumentException("domainValue");
		}

		public object Deserialize(object value, IDomainType domainType)
		{
			var edmValue = value as IEdmValue;
			if (edmValue != null)
			{
				string name = this.converter.AsClrValue<string>(edmValue);

				IMetadataItem metadataItem;
				object metadataItemValue;
				if (this.metadataService.TryGetMetadataItem(name, out metadataItem) &&
					domainType.TryParse(metadataItem, CultureInfo.InvariantCulture, out metadataItemValue))
				{
					return metadataItemValue;
				}
			}

			throw new ArgumentException("value");
		}
	}
}