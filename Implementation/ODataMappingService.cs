// <copyright file="ODataMappingService.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain
{
	using System;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Repository;

	public class ODataMappingService : IODataMappingService
	{
		private readonly IRepositoryFactory repositoryFactory;

		private bool initialized;

		private IODataRepository oDataRepository;

		public ODataMappingService(IRepositoryFactory repositoryFactory)
		{
			this.repositoryFactory = repositoryFactory;
		}

		public void InitializeMapping()
		{
			if (this.initialized)
			{
				throw new InvalidOperationException("Mapping is already initialized");
			}

			this.oDataRepository = this.repositoryFactory.CreateODataRepository();
			this.initialized = true;
		}

		public IODataRepository GetODataRepository()
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException("Mapping hasn't been initialized");
			}

			return this.oDataRepository;
		}
	}
}