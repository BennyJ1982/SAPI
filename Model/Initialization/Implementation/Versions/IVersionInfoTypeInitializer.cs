namespace Facton.ServiceApi.Domain.Model.Initialization.Versions
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	public interface IVersionInfoTypeInitializer
	{
		IEntityTypeBuilder GetOrCreateVersionInfoType(IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityTypeBuilder);

		void InitializeVersionsNavigationProperty(
			IBindableModelBuilder modelBuilder,
			IEntityTypeBuilder sourceTypeBuilder,
			IEntityTypeBuilder versionInfoTypeBuilder);
	}
}