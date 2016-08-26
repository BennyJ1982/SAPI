namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Queries;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	/// <summary>
	/// Initializes the model with entity sets for facton signatures
	/// </summary>
	public class SignatureEntitySetInitializer : ISignatureEntitySetInitializer
	{
		private readonly IMetadataService metadataService;

		private readonly IPluralizationService pluralizationService;

		private readonly IFactonQueryService factonQueryService;

		private readonly IQueryBuilderFactory queryBuilderFactory;

		public SignatureEntitySetInitializer(
			IMetadataService metadataService,
			IPluralizationService pluralizationService,
			IFactonQueryService factonQueryService,
			IQueryBuilderFactory queryBuilderFactory)
		{
			this.metadataService = metadataService;
			this.pluralizationService = pluralizationService;
			this.factonQueryService = factonQueryService;
			this.queryBuilderFactory = queryBuilderFactory;
		}

		public IEntitySetBuilder InitializeSet(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder entityTypeBuilder,
			ISignature signature,
			IEnumerable<string> entityTypes)
		{
			return this.InitializeSet(modelBuilder, entityTypeBuilder, signature, entityTypes, string.Empty);
		}

		public IEntitySetBuilder InitializeSingleton(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder entityTypeBuilder,
			ISignature signature,
			IEnumerable<string> entityTypes,
			string space)
		{
			var singletonBuilder =
				modelBuilder.CreateEntitySetBuilder(this.GetSetName(signature), entityTypeBuilder)
					.AsSingleton()
					.WithSupportedOperations(Operation.Get | Operation.Patch);

			modelBuilder.WithBinding(
				singletonBuilder,
				new SignatureSingletonBinding(this.factonQueryService, this.queryBuilderFactory, signature, entityTypes, space));

			return singletonBuilder;
		}

		public IEntitySetBuilder InitializeSet(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder entityTypeBuilder,
			ISignature signature,
			IEnumerable<string> entityTypes,
			string space)
		{
			var entitySetBuilder =
				modelBuilder.CreateEntitySetBuilder(this.GetSetName(signature), entityTypeBuilder)
					.WithSupportedOperations(Operation.Get | Operation.Post | Operation.Patch);

			modelBuilder.WithBinding(entitySetBuilder, this.CreateEntitySetBinding(signature, entityTypes, space));
			return entitySetBuilder;
		}

		public IEnumerable<ISignature> GetRelevantSignaturesByType(string signatureType)
		{
			return this.metadataService.CompactSignatures.Where(IsRelevantSignature).Where(signature => signature.SignatureType == signatureType);
		}

		public string GetSetName(ISignature signature)
		{
			return this.pluralizationService.Pluralize(signature.Name);
		}

		private SignatureEntitySetBinding CreateEntitySetBinding(ISignature signature, IEnumerable<string> entityTypes, string space)
		{
			return string.IsNullOrEmpty(space)
						? new SignatureEntitySetBinding(this.factonQueryService, this.queryBuilderFactory, signature, entityTypes)
						: new SignatureEntitySetBinding(this.factonQueryService, this.queryBuilderFactory, signature, entityTypes, space);
		}

		private static bool IsRelevantSignature(ISignature signature)
		{
			return signature.IsPublic;
		}
	}
}
