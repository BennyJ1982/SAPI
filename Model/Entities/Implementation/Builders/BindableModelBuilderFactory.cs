namespace Facton.ServiceApi.Domain.Model.Entities.Builders
{
	using Facton.ServiceApi.Domain.Model.Core.Builders;

	public class BindableModelBuilderFactory : IBindableModelBuilderFactory
	{
		private readonly IModelBuilderFactory modelBuilderFactory;

		public BindableModelBuilderFactory(IModelBuilderFactory modelBuilderFactory)
		{
			this.modelBuilderFactory = modelBuilderFactory;
		}

		public IBindableModelBuilder Create()
		{
			return new BindableModelBuilder(this.modelBuilderFactory.Create());
		}
	}
}
