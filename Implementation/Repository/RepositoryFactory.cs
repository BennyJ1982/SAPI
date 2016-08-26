// <copyright file="RepositoryFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Repository
{
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Builders;
	using Facton.ServiceApi.Domain.Model.Initialization;

	/// <summary>
	/// Creates the odata repository
	/// </summary>
	internal class RepositoryFactory : IRepositoryFactory
	{
		private readonly IFactonModelInitializer factonModelInitializer;

		private readonly IOperationContextFactory operationContextFactory;

		private readonly IBindableModelBuilderFactory modelBuilderFactory;

		public RepositoryFactory(
			IFactonModelInitializer factonModelInitializer,
			IOperationContextFactory operationContextFactory,
			IBindableModelBuilderFactory modelBuilderFactory)
		{
			this.factonModelInitializer = factonModelInitializer;
			this.operationContextFactory = operationContextFactory;
			this.modelBuilderFactory = modelBuilderFactory;
		}

		public IODataRepository CreateODataRepository()
		{
			var modelBuilder = this.modelBuilderFactory.Create();
			this.factonModelInitializer.Initialize(modelBuilder);

			return new ODataRepository(modelBuilder.Build(), this.operationContextFactory);
		}
	}
}