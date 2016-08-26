// <copyright file="ModelContext.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Edm;

	public class ModelContext : IModelContext
	{
		private readonly IDictionary<string, IEntityType> entityTypes;

		private readonly IDictionary<string, IEntitySet> entitySets;

		private readonly IODataPathService pathService;

		private readonly OperationHandlerStore operationHandlerStore;

		public ModelContext(
			IEdmModel edmModel,
			IDictionary<string, IEntityType> entityTypes,
			IDictionary<string, IEntitySet> entitySets,
			IODataPathService pathService,
			OperationHandlerStore operationHandlerStore)
		{
			this.EdmModel = edmModel;
			this.entityTypes = entityTypes;
			this.entitySets = entitySets;
			this.pathService = pathService;
			this.operationHandlerStore = operationHandlerStore;
		}

		public IEdmModel EdmModel { get; }

		public bool TryGetEntityType(string name, out IEntityType entityType)
		{
			return this.entityTypes.TryGetValue(name, out entityType);
		}

		public bool TryGetEntitySet(string name, out IEntitySet entitySet)
		{
			return this.entitySets.TryGetValue(name, out entitySet);
		}

		public bool TryGetHandler(IOperationContext operationContext, out IOperationHandler relevantHandler)
		{
			relevantHandler =
				this.operationHandlerStore.GetHandlersForNavigationTarget(operationContext.NavigationTarget)
					.FirstOrDefault(handler => handler.CanHandle(operationContext));
			return relevantHandler != null;
		}

		public INavigatable GetNavigationTarget(ODataPath path)
		{
			return this.pathService.GetNavigationTarget(this, path);
		}
	}
}