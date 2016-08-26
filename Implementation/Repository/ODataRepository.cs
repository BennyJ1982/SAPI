// <copyright file="ODataRepository.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Edm;

	internal class ODataRepository : IODataRepository
	{
		private readonly IModelContext modelContext;

		private readonly IOperationContextFactory operationContextFactory;

		public ODataRepository(IModelContext modelContext, IOperationContextFactory operationContextFactory)
		{
			this.modelContext = modelContext;
			this.operationContextFactory = operationContextFactory;
		}

		public IEdmModel GetModel() => this.modelContext.EdmModel;

		public bool CanPost(ODataPath path)
		{
			return this.modelContext.GetNavigationTarget(path).SupportedOperations.HasFlag(Operation.Post);
		}

		public bool CanPatch(ODataPath path)
		{
			return this.modelContext.GetNavigationTarget(path).SupportedOperations.HasFlag(Operation.Patch);
		}

		public bool CanGet(ODataPath path)
		{
			return this.modelContext.GetNavigationTarget(path).SupportedOperations.HasFlag(Operation.Get);
		}

		public Task<ODataEntityDto> Post(ODataPath path, QueryOptions queryOptions, ODataEntityDto incomingObject)
		{
			var result = this.Execute(Operation.Post, path, queryOptions, incomingObject);
			return Task.FromResult(result.FirstOrDefault());
		}

		public Task<ODataEntityDto> Patch(ODataPath path, QueryOptions queryOptions, ODataEntityDto incomingObject)
		{
			var result = this.Execute(Operation.Patch, path, queryOptions, incomingObject);
			return Task.FromResult(result.FirstOrDefault());
		}

		public Task<IEnumerable<ODataEntityDto>> Get(ODataPath path, QueryOptions queryOptions)
		{
			return Task.FromResult(this.Execute(Operation.Get, path, queryOptions, null));
		}

		private IEnumerable<ODataEntityDto> Execute(
			Operation operation, ODataPath path, QueryOptions queryOptions, ODataEntityDto incomingObject)
		{
			var operationContext = this.operationContextFactory.Create(this.modelContext, operation, path, queryOptions);
			IOperationHandler relevantHandler;
			if (!this.modelContext.TryGetHandler(operationContext, out relevantHandler))
			{
				throw new InvalidOperationException($"No handler found that could handle the {operation} operation on {path.Last()}.");
			}

			// We need to make this call synchronous in the current thread, because otherwise we might not have a unit of work
			var operationResult = relevantHandler.Handle(operationContext, incomingObject).ToArray();
			return operationResult;
		}
	}
}