namespace Facton.ServiceApi.Domain.Model.Initialization
{
	public interface IPluralizationService
	{
		string Pluralize(string word);

		string Singularize(string word);
	}
}
