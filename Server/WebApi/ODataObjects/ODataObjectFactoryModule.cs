namespace ServiceApi.Server.WebApi.ODataObjects
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using ServiceApi.Model.Core.Serialization;

	/// <summary>
	/// Module that provides the <see cref="IODataObjectFactory"/>
	/// </summary>
	public class ODataObjectFactoryModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			typeRegistry.RegisterInstance<IODataObjectFactory>(new WebApiODataObjectFactory());
		}
	}
}