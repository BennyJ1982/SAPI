// <copyright file="RegistriesModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	/// <summary>
	/// Module that provides registries for model initialization
	/// </summary>
	public class RegistriesModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			typeRegistry.RegisterInstance<IModelConfigurationRegistry>(new ModelConfigurationRegistry());
			typeRegistry.RegisterInstance<IEntityTypeInitializerRegistry>(new EntityTypeInitializerRegistry());
		}
	}
}