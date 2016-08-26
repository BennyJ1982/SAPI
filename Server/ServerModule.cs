// <copyright file="ServerModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace ServiceApi.Server
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using ServiceApi.Server.Hosting;
	using ServiceApi.Server.Middleware;
	using ServiceApi.Server.WebApi;

	/// <summary>
	/// Module that starts the web server
	/// </summary>
	public class ServerModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			// initialize mapping
			var mappingService = typeRegistry.GetObject<IODataMappingService>();
			mappingService.InitializeMapping();

			// create and start host
			var config = WebApiInitializer.Configure(mappingService.GetODataRepository(), new ConsoleLogger());
			var host = new HttpHost(config);
			typeRegistry.RegisterInstance<IHttpHost>(host);

			host.Open("http://local.facton.local");
		}
	}
}