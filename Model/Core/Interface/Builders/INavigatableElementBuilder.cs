namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	public interface INavigatableElementBuilder
	{
		bool AlreadyBuilt { get; }

		INavigatable BuiltElement { get; }
	}
}
