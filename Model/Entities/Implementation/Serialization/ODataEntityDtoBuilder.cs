// <copyright file="ODataEntityDtoBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Serialization
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Metadata.DomainValues;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Annotations;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;

	using Microsoft.OData.Core;

	/// <summary>
	/// Builds an odata entity dto by applying structural and expanded navigation properties to it.
	/// </summary>
	public class ODataEntityDtoBuilder : IODataEntityDtoBuilder
	{
		private readonly IODataEntityDtoBuilderFactory dtoBuilderFactory;

		private readonly List<ODataProperty> structuralProperties = new List<ODataProperty>();

		private readonly List<ODataNavigationProperty> navigationProperties = new List<ODataNavigationProperty>();

		public ODataEntityDtoBuilder(IODataObjectFactory objectFactory, IODataEntityDtoBuilderFactory dtoBuilderFactory, IEntityType entityType)
		{
			this.dtoBuilderFactory = dtoBuilderFactory;
			this.EntityType = entityType;
			this.DtoUnderConstruction = objectFactory.CreateODataEntityDto(entityType.ResultingEdmEntityType);
			this.DtoUnderConstruction.NavigationProperties = this.navigationProperties;
			this.DtoUnderConstruction.Entry.Properties = this.structuralProperties;
		}

		public ODataEntityDto DtoUnderConstruction { get; }

		public IEntityType EntityType { get; }

		public void ApplyAllStructuralPropertiesAndKeys(IEntity sourceEntity)
		{
			foreach (var property in this.EntityType.GetStructuralAndKeyProperties())
			{
				this.ApplyStructuralProperty(property, sourceEntity);
			}
		}

		public void ApplyStructuralProperty(IStructuralEntityProperty property, IEntity sourceEntity)
		{
			var odataProperty = new ODataProperty { Name = property.EdmPropertyName };

			object value;
			if (property.TryGetValueFromEntity(sourceEntity, out value))
			{
				odataProperty.Value = property.Serialize(value);

				var attributedValue = value as IAttributedValue;
				if (attributedValue != null && !attributedValue.IsSignificant())
				{
					odataProperty.InstanceAnnotations = new[] { CreateNullValueAnnotation(NullValueAnnotationType.NotSignificant) };
				}
				else if (value is InvalidValue)
				{
					odataProperty.InstanceAnnotations = new[] { CreateNullValueAnnotation(NullValueAnnotationType.NotValid) };
				}
			}
			else
			{
				odataProperty.InstanceAnnotations = new[] { CreateNullValueAnnotation(NullValueAnnotationType.NotProvided) };
			}

			this.structuralProperties.Add(odataProperty);
		}

		public void ApplyNavigationProperty(
			INavigationProperty navigationProperty,
			IEnumerable<IEntity> entities,
			Action<IODataEntityDtoBuilder, IEntity> childDtoInitializer)
		{
			if (!navigationProperty.IsCollection())
			{
				var singleEntity = entities.SingleOrDefault();
				if (singleEntity != null)
				{
					var childDtoBuilder = this.dtoBuilderFactory.Create(navigationProperty.TargetType);
					childDtoInitializer(childDtoBuilder, singleEntity);

					this.navigationProperties.Add(
						new ODataNavigationProperty { Name = navigationProperty.Name, Value = childDtoBuilder.DtoUnderConstruction });
				}
			}
			else
			{
				var childDtoBuilders = new List<IODataEntityDtoBuilder>();
				foreach (var entity in entities)
				{
					var childDtoBuilder = this.dtoBuilderFactory.Create(navigationProperty.TargetType);
					childDtoInitializer(childDtoBuilder, entity);
					childDtoBuilders.Add(childDtoBuilder);
				}

				var childDtos = childDtoBuilders.Select(b => b.DtoUnderConstruction).ToArray();
				this.navigationProperties.Add(new ODataNavigationProperty { Name = navigationProperty.Name, Value = childDtos });
			}
		}

		private static ODataInstanceAnnotation CreateNullValueAnnotation(NullValueAnnotationType annotationType)
		{
			const string AnnotationNamespace = "Facton.Values.NullValue";
			return new ODataInstanceAnnotation(AnnotationNamespace, new ODataPrimitiveValue(AnnotationNamespace + $"’{annotationType}’"));
		}
	}
}