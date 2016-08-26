namespace Facton.ServiceApi.Domain.Model.Initialization.Global
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	public class GlobalSignatureTypeInitializer : ISignatureTypeInitializer
	{
		private readonly ISignatureEntitySetInitializer signatureSetInitializer;

		public GlobalSignatureTypeInitializer(ISignatureEntitySetInitializer signatureSetInitializer)
		{
			this.signatureSetInitializer = signatureSetInitializer;
		}

		public bool CanHandleSignatureType(SignatureTypeConfiguration config)
		{
			return config.SignatureType == "Global";
		}

		public IEntityTypeBuilder Initialize(
			SignatureTypeConfiguration config,
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder publicEntityType)
		{
			var typeBuilder = modelBuilder.CreateEntityTypeBuilder(config.SignatureType).WithParentType(publicEntityType);
			foreach (var signature in this.signatureSetInitializer.GetRelevantSignaturesByType(config.SignatureType))
			{
				this.signatureSetInitializer.InitializeSingleton(
					modelBuilder, typeBuilder, signature, config.RelevantEntityTypes, "GlobalMasterData");
			}

			return typeBuilder;
		}
	}
}
