// <copyright file="PublicEntityTypeInitializer.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System;
	using System.Diagnostics;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Common;

	public class PublicEntityTypeInitializer : IPublicEntityTypeInitializer
	{
		private readonly IPropertyService propertyService;

		private readonly IStructuralPropertyFactory structuralPropertyFactory;

		private readonly IMappingLogger logger;

		public PublicEntityTypeInitializer(
			IPropertyService propertyService,
			IStructuralPropertyFactory structuralPropertyFactory,
			IMappingLogger logger)
		{
			this.propertyService = propertyService;
			this.structuralPropertyFactory = structuralPropertyFactory;
			this.logger = logger;
		}

		public IEntityTypeBuilder Initialize(IBindableModelBuilder modelBuilder)
		{
			var signatureProperty = this.CreateProperty(this.propertyService.CompactSignatureProperty, FactonModelKeywords.SignaturePropertyName);

			var publicEntityType =
				modelBuilder.CreateEntityTypeBuilder(FactonModelKeywords.PublicEntityTypeName)
					.AsAbstractType()
					.WithKeyProperty(CreateEntityIdProperty(this.structuralPropertyFactory))
					.WithStructuralProperty(signatureProperty);

			foreach (var property in this.propertyService.AllRelevantProperties)
			{
				IStructuralProperty structuralProperty;
				if (this.structuralPropertyFactory.TryCreate(property, out structuralProperty))
				{
					publicEntityType.WithStructuralProperty(structuralProperty);
				}
				else
				{
					this.logger.Write(TraceEventType.Error, $"Cannot create property {property.Name}: data type {property.DomainType} not mapped");
				}
			}

			return publicEntityType;
		}

		private static IStructuralProperty CreateEntityIdProperty(IStructuralPropertyFactory structuralPropertyFactory)
		{
			IStructuralProperty entityIdProperty;
			if (structuralPropertyFactory.TryCreate(FactonModelKeywords.IdPropertyName, e => e.Id, false, out entityIdProperty))
			{
				return entityIdProperty;
			}
			throw new InvalidOperationException("Could not map entity id property.");
		}

		private IStructuralProperty CreateProperty(IProperty property, string edmPropertyName)
		{
			IStructuralProperty structuralProperty;
			if (this.structuralPropertyFactory.TryCreate(property, edmPropertyName, out structuralProperty))
			{
				return structuralProperty;
			}

			throw new InvalidOperationException("Cannot map property: " + edmPropertyName);
		}
	}
}