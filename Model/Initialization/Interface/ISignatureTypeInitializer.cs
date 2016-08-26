namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	public interface ISignatureTypeInitializer : IEntityTypeInitializer
	{
		bool CanHandleSignatureType(SignatureTypeConfiguration config);

		IEntityTypeBuilder Initialize(SignatureTypeConfiguration config, IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType);
	}
}
