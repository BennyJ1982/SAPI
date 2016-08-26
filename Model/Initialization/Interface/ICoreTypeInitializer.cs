namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	public interface ICoreTypeInitializer : IEntityTypeInitializer
	{
		IEntityTypeBuilder Initialize(IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType);
	}
}
