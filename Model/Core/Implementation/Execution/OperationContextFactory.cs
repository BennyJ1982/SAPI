// <copyright file="OperationContextFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using Facton.ServiceApi.Core;
	using Microsoft.OData.Core.UriParser.Semantic;

	public class OperationContextFactory : IOperationContextFactory
	{
		private readonly IODataPathService pathService;

		public OperationContextFactory(IODataPathService pathService)
		{
			this.pathService = pathService;
		}

		public IOperationContext Create(IModelContext modelContext, Operation operation, ODataPath path, QueryOptions queryOptions)
		{
			var navigationTarget = this.pathService.GetNavigationTarget(modelContext, path);
			var navigationRoot = this.pathService.GetNavigationRoot(modelContext, path);
			return new OperationContext(path, operation, queryOptions, navigationTarget, navigationRoot, modelContext);
		}
	}
}