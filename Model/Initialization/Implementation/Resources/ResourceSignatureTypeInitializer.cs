namespace Facton.ServiceApi.Domain.Model.Initialization.Resources
{
	using System;
	using System.Linq;

	using Facton.Domain.Resources;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Initializer for the Resource signature type
	/// </summary>
	public class ResourceSignatureTypeInitializer : ISignatureTypeInitializer
	{
		private readonly ISignatureEntitySetInitializer signatureSetInitializer;

		private readonly IResourceService resourceService;

		private readonly IStructuralProperty quantityTypeProperty;

		public ResourceSignatureTypeInitializer(
			ISignatureEntitySetInitializer signatureSetInitializer,
			IResourceService resourceService,
			IStructuralProperty quantityTypeProperty)
		{
			this.signatureSetInitializer = signatureSetInitializer;
			this.resourceService = resourceService;
			this.quantityTypeProperty = quantityTypeProperty;
		}

		public bool CanHandleSignatureType(SignatureTypeConfiguration config)
		{
			return config.SignatureType == "Resource";
		}

		public IEntityTypeBuilder Initialize(SignatureTypeConfiguration config, IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType)
		{
			var typeBuilder = modelBuilder.CreateEntityTypeBuilder(config.SignatureType)
				.WithParentType(publicEntityType)
				.WithStructuralProperty(this.quantityTypeProperty);

			foreach (var signature in this.signatureSetInitializer.GetRelevantSignaturesByType(config.SignatureType))
			{
				var setBuilder = this.signatureSetInitializer.InitializeSet(
					modelBuilder,
					typeBuilder,
					signature,
					config.RelevantEntityTypes,
					"ResourceMasterdata");

				IEntitySetBinding existingBinding;
				if (!modelBuilder.TryGetBinding(setBuilder, out existingBinding))
				{
					throw new InvalidOperationException("No existing bind found for resource entity set.");
				}

				modelBuilder
					.WithBinding(setBuilder, new ResourceEntitySetBinding(this.resourceService, signature, existingBinding))
					.WithDependency(setBuilder, this.quantityTypeProperty)
					.WithOptionalDependency(setBuilder, publicEntityType.KeyProperties.First());
			}

			return typeBuilder;
		}
	}
}
