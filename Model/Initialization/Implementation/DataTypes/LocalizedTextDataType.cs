// <copyright file="LocalizedTextDataType.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.DataTypes
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Metadata.ContentLocalization;
	using Facton.Infrastructure.Metadata.DomainValues;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;

	using Microsoft.OData.Core;
	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;

	public class LocalizedTextDataType : IDomainTypeDependingDataType, IComplexDataType
	{
		private readonly IODataObjectFactory oDataObjectFactory;

		private readonly EdmComplexType edmComplexType;
		private readonly EdmComplexTypeReference cultureValueTypeReference;
		private readonly EdmCollectionTypeReference cultureValuesCollectionTypeReference;

		public LocalizedTextDataType(IODataObjectFactory oDataObjectFactory)
		{
			this.oDataObjectFactory = oDataObjectFactory;
			this.cultureValueTypeReference = CreateCultureValueTypeReference();
			this.cultureValuesCollectionTypeReference = CreateCultureValuesCollectionTypeReference(this.cultureValueTypeReference);

			this.edmComplexType = CreateComplexType(this.cultureValuesCollectionTypeReference);
		}

		public IEdmTypeReference GetEdmTypeReference(bool nullable = true)
		{
			return new EdmComplexTypeReference(this.edmComplexType, nullable);
		}

		public void AddToModel(EdmModel model)
		{
			model.AddElement((IEdmSchemaElement)this.cultureValueTypeReference.Definition);
			model.AddElement(this.edmComplexType);
		}

		public object Serialize(object domainValue, IDomainType domainType)
		{
			var localizedValue = domainValue as ILocalizedTextValue;
			if (localizedValue != null)
			{
				// culture specific values
				var cultureValues = new List<ODataComplexValue>();
				foreach (var culture in localizedValue.ProvidedLanguages)
				{
					var cultureValue = this.oDataObjectFactory.CreateODataComplexValue(this.cultureValueTypeReference.ComplexDefinition());
					cultureValue.Properties = new[]
												{
													new ODataProperty { Name = "Culture", Value = culture.Name },
													new ODataProperty { Name = "Value", Value = localizedValue.GetValue(culture).Text }
												};

					cultureValues.Add(cultureValue);
				}

				var complexValue = this.oDataObjectFactory.CreateODataComplexValue(this.edmComplexType);
				var collectionValue = this.oDataObjectFactory.CreateODataCollectionValue(this.cultureValuesCollectionTypeReference.CollectionDefinition());
				collectionValue.Items = cultureValues;
				var complexValueProperties = new List<ODataProperty>
				{
					new ODataProperty
						{
							Name = "CultureValues",
							Value = collectionValue
						},
				};

				// invariant value
				ITextValue invariantValue;
				if (localizedValue.TryGetInvariantValue(out invariantValue))
				{
					complexValueProperties.Add(new ODataProperty { Name = "InvariantValue", Value = invariantValue.Text });
				}

				complexValue.Properties = complexValueProperties;

				return complexValue;
			}

			if (domainValue != null)
			{
				return null; // unknown value, but it might be handled by instance annotations
			}

			throw new ArgumentException("domainValue");
		}

		public object Deserialize(object value, IDomainType domainType)
		{
			// TODO handle cases where this value gets sets to null (insignificant)

			var localizedValue = domainType.EmptyValue as ILocalizedTextValue;
			if (localizedValue == null)
			{
				throw new ArgumentException("defaultValue");
			}

			var complexObject = value as ODataComplexValue;
			if (complexObject == null)
			{
				throw new ArgumentException("value");
			}

			var attributedValue = localizedValue.As<IAttributedValue>();
			localizedValue = attributedValue.RemoveAttributes(attributedValue.Attributes).As<ILocalizedTextValue>();

			// invariant value
			string invariantText;
			if (complexObject.TryGetPropertyValue("InvariantValue", out invariantText))
			{
				localizedValue = localizedValue.SetInvariantValue(invariantText ?? string.Empty);
			}

			// culture dependent values
			ODataCollectionValue cultureValues;
			if (complexObject.TryGetPropertyValue("CultureValues", out cultureValues) && cultureValues.Items.Any())
			{
				foreach (var cultureValue in cultureValues.Items.Cast<ODataComplexValue>())
				{
					string culture;
					string text;
					cultureValue.TryGetPropertyValue("Culture", out culture);
					cultureValue.TryGetPropertyValue("Value", out text);

					localizedValue = localizedValue.SetValue(CultureInfo.GetCultureInfo(culture), text);
				}
			}

			return localizedValue;
		}

		private static EdmComplexType CreateComplexType(EdmCollectionTypeReference valuesCollectionTypeReference)
		{
			var type = new EdmComplexType("facton", "LocalizedText");
			type.AddStructuralProperty("InvariantValue", EdmPrimitiveTypeKind.String, true);
			type.AddStructuralProperty("CultureValues", valuesCollectionTypeReference);

			return type;
		}

		private static EdmCollectionTypeReference CreateCultureValuesCollectionTypeReference(IEdmComplexTypeReference cultureValueTypeReference)
		{
			var collectionType = new EdmCollectionType(cultureValueTypeReference);
			return new EdmCollectionTypeReference(collectionType);
		}

		private static EdmComplexTypeReference CreateCultureValueTypeReference()
		{
			var cultureValueType = new EdmComplexType("facton", "CultureValue");
			cultureValueType.AddStructuralProperty("Culture", EdmPrimitiveTypeKind.String);
			cultureValueType.AddStructuralProperty("Value", EdmPrimitiveTypeKind.String);

			return new EdmComplexTypeReference(cultureValueType, false);
		}
	}
}