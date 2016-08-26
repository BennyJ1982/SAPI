// <copyright file="LoggingModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace ServiceApi.Server.Middleware
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using ServiceApi.Model.Core.Serialization;

	/// <summary>
	/// Module that provides the mapping logger.
	/// </summary>
	public class LoggingModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			// provide mapping service
			typeRegistry.RegisterInstance<IMappingLogger>(new ConsoleLogger());
		}
	}
}