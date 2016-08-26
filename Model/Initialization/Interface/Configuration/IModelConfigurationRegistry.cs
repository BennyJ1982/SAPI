// <copyright file="IModelConfigurationRegistry.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Configuration
{
	using System.Collections.Generic;

	public interface IModelConfigurationRegistry
	{
		IEnumerable<SignatureTypeConfiguration> RegisteredSignatureTypeConfigurations { get; }

		void RegisterSignatureTypeConfiguration(SignatureTypeConfiguration supportedFactonSignatureType);
	}
}