namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	/// <summary>
	/// The initializer for the public entity type.
	/// </summary>
	public interface IPublicEntityTypeInitializer
	{
		IEntityTypeBuilder Initialize(IBindableModelBuilder modelBuilder);
	}
}