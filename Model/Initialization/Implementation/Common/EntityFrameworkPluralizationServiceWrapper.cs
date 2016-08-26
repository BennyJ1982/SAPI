namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Data.Entity.Design.PluralizationServices;

	public class EntityFrameworkPluralizationServiceWrapper : IPluralizationService
	{
		private readonly PluralizationService pluralizationService;

		public EntityFrameworkPluralizationServiceWrapper(PluralizationService pluralizationService)
		{
			this.pluralizationService = pluralizationService;
		}

		public string Pluralize(string word)
		{
			return this.pluralizationService.Pluralize(word);
		}

		public string Singularize(string word)
		{
			return this.pluralizationService.Singularize(word);
		}
	}
}
