// <copyright file="SignatureTypeConfiguration.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Configuration
{
	/// <summary>
	/// Holds information about a FACTON signature type supported by the ODATA Model
	/// </summary>
	public class SignatureTypeConfiguration
	{
		public SignatureTypeConfiguration(string signatureType, string[] relevantEntityTypes)
		{
			this.SignatureType = signatureType;
			this.RelevantEntityTypes = relevantEntityTypes;
		}

		public string SignatureType { get; }

		public string[] RelevantEntityTypes { get; }
	}
}