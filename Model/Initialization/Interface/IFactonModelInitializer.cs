namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.ServiceApi.Domain.Model.Entities.Builders;

	/// <summary>
	/// Initializes the facton model
	/// </summary>
	public interface IFactonModelInitializer
	{
		void Initialize(IBindableModelBuilder modelBuilder);
	}
}
