namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using Facton.Infrastructure.Metadata;

	public interface ICostSharpNavigationPropertyBuilder : INavigationPropertyBuilder
	{
		IProperty SourceProperty { get; }
	}
}