namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System.Collections.Generic;

	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	/// <summary>
	/// A model context that supports bindings.
	/// </summary>
	public interface IBindableModelContext : IModelContext
	{
		bool TryGetBinding<T>(INavigatable navigatable, out T binding) where T : class, IBinding;

		IEnumerable<DependencyDeclaration> GetDependencies(INavigatable navigatable);
	}
}
