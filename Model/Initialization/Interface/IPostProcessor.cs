namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;

	public interface IPostProcessor : IEntityTypeInitializer
	{
		void PerformPostProcessing(IModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType);
	}
}