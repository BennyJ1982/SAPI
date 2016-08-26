// <copyright file="ModelConfigurationRegistry.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Collections.Generic;
	using Facton.ServiceApi.Domain.Model.Initialization.Configuration;

	public class ModelConfigurationRegistry : IModelConfigurationRegistry
	{
		private readonly List<SignatureTypeConfiguration> registeredSignatureTypeConfigurations = new List<SignatureTypeConfiguration>();

		public IEnumerable<SignatureTypeConfiguration> RegisteredSignatureTypeConfigurations => this.registeredSignatureTypeConfigurations;

		public void RegisterSignatureTypeConfiguration(SignatureTypeConfiguration supportedFactonSignatureType)
		{
			this.registeredSignatureTypeConfigurations.Add(supportedFactonSignatureType);
		}
	}
}