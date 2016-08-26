namespace Facton.ServiceApi.Domain.Model.Initialization.Versions
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Initializer for signature types whose entities can have versions (calculations, variants)
	/// TODO: Could be separated at some point
	/// </summary>
	public class VersionSupportingSignatureTypeInitializer : ISignatureTypeInitializer
	{
		private readonly ISignatureEntitySetInitializer signatureSetInitializer;

		private readonly IVersionInfoTypeInitializer versionInfoTypeInitializer;

		public VersionSupportingSignatureTypeInitializer(
			ISignatureEntitySetInitializer signatureSetInitializer,
			IVersionInfoTypeInitializer versionInfoTypeInitializer)
		{
			this.signatureSetInitializer = signatureSetInitializer;
			this.versionInfoTypeInitializer = versionInfoTypeInitializer;
		}

		public bool CanHandleSignatureType(SignatureTypeConfiguration config)
		{
			return config.SignatureType == "Calculation" | config.SignatureType == "VariantFolder";
		}

		public IEntityTypeBuilder Initialize(
			SignatureTypeConfiguration config,
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder publicEntityType)
		{
			var typeBuilder = modelBuilder.CreateEntityTypeBuilder(config.SignatureType).WithParentType(publicEntityType);

			var versionInfoTypeBuilder = this.versionInfoTypeInitializer.GetOrCreateVersionInfoType(modelBuilder, publicEntityType);
			this.versionInfoTypeInitializer.InitializeVersionsNavigationProperty(modelBuilder, typeBuilder, versionInfoTypeBuilder);

			foreach (var signature in this.signatureSetInitializer.GetRelevantSignaturesByType(config.SignatureType))
			{
				this.signatureSetInitializer.InitializeSet(modelBuilder, typeBuilder, signature, config.RelevantEntityTypes);
			}

			return typeBuilder;
		}
	}
}
