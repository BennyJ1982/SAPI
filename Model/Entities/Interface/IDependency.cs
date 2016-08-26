namespace Facton.ServiceApi.Domain.Model.Entities
{
	public interface IDependency
	{
		object DependableElement { get; }

		object Value { get; }
	}
}
