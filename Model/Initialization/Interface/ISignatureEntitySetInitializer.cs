namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	public interface ISignatureEntitySetInitializer
	{
		IEntitySetBuilder InitializeSet(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder entityTypeBuilder,
			ISignature signature,
			IEnumerable<string> entityTypes,
			string space);

		IEntitySetBuilder InitializeSet(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder entityTypeBuilder,
			ISignature signature,
			IEnumerable<string> entityTypes);

		IEntitySetBuilder InitializeSingleton(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder entityTypeBuilder,
			ISignature signature,
			IEnumerable<string> entityTypes,
			string space);

		IEnumerable<ISignature> GetRelevantSignaturesByType(string signatureType);

		string GetSetName(ISignature signature);
	}
}