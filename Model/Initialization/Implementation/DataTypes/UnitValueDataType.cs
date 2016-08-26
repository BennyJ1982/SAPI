// <copyright file="UnitValueDataType.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.DataTypes
{
	using System;
	using System.Globalization;

	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Metadata.Measurement;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;

	using Microsoft.OData.Core;
	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;

	public class UnitValueDataType : IDomainTypeDependingDataType, IComplexDataType
	{
		private const string NumberPropertyName = "Number";

		private const string UnitPropertyName = "Unit";

		private readonly IODataObjectFactory oDataObjectFactory;

		private readonly EdmComplexType edmComplexType;

		public UnitValueDataType(IODataObjectFactory oDataObjectFactory)
		{
			this.oDataObjectFactory = oDataObjectFactory;
			this.edmComplexType = CreateEdmComplexType();
		}

		public IEdmTypeReference GetEdmTypeReference(bool nullable = true)
		{
			return new EdmComplexTypeReference(this.edmComplexType, nullable);
		}

		public void AddToModel(EdmModel model)
		{
			model.AddElement(this.edmComplexType);
		}

		public object Serialize(object domainValue, IDomainType domainType)
		{
			var unitValue = domainValue as IUnitValue;
			if (unitValue != null)
			{
				var complexObject = this.oDataObjectFactory.CreateODataComplexValue(this.edmComplexType);
				complexObject.Properties = new[] {
					new ODataProperty
					{
						Name = NumberPropertyName,
						Value = unitValue.InternalNumber
					},
					new ODataProperty
					{
						Name = UnitPropertyName,
						Value = unitValue.UnitRepresentation
					} };

				return complexObject;
			}

			if (domainValue != null)
			{
				return null; // unknown value, but it might be handled by instance annotations
			}

			throw new ArgumentException("domainValue");
		}

		public object Deserialize(object value, IDomainType domainType)
		{
			var complexObject = value as ODataComplexValue;
			if (complexObject == null)
			{
				throw new ArgumentException("value");
			}

			decimal number;
			if (!complexObject.TryGetPropertyValue(NumberPropertyName, out number))
			{
				throw new ArgumentException("value");
			}

			string unitRepresentation;
			if (!complexObject.TryGetPropertyValue(UnitPropertyName, out unitRepresentation))
			{
				throw new ArgumentException("value");
			}

			object unitValue;
			if (!domainType.TryParse(string.Concat(number, " ", unitRepresentation), CultureInfo.InvariantCulture, out unitValue))
			{
				throw new ArgumentException("value");
			}

			return unitValue;
		}

		private static EdmComplexType CreateEdmComplexType()
		{
			var type = new EdmComplexType("facton", "UnitValue");
			type.AddStructuralProperty(NumberPropertyName, EdmPrimitiveTypeKind.Decimal);
			type.AddStructuralProperty(UnitPropertyName, EdmPrimitiveTypeKind.String);
			return type;
		}
	}
}