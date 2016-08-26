// <copyright file="OperationHandlerStore.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using System.Collections.Generic;
	using System.Linq;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;

	public class OperationHandlerStore
	{
		private readonly IDictionary<INavigatable, IList<IOperationHandler>> operationHandlers =
			new Dictionary<INavigatable, IList<IOperationHandler>>();

		private readonly IEnumerable<IOperationHandler> globalOperationHandlers;

		public OperationHandlerStore(
			IEnumerable<KeyValuePair<INavigatable, IOperationHandler>> operationHandlers,
			IEnumerable<IOperationHandler> globalOperationHandlers)
		{
			foreach (var handlerPair in operationHandlers)
			{
				IList<IOperationHandler> handlers;
				if (!this.operationHandlers.TryGetValue(handlerPair.Key, out handlers))
				{
					this.operationHandlers[handlerPair.Key] = handlers = new List<IOperationHandler>();
				}

				handlers.Add(handlerPair.Value);
			}

			this.globalOperationHandlers = globalOperationHandlers;
		}

		public IEnumerable<IOperationHandler> GetHandlersForNavigationTarget(INavigatable navigationTarget)
		{
			IList<IOperationHandler> navigationTargetHandlers;
			if (this.operationHandlers.TryGetValue(navigationTarget, out navigationTargetHandlers))
			{
				return navigationTargetHandlers.OrderBy(h => h.Rank).Union(this.globalOperationHandlers.OrderBy(h => h.Rank));
			}

			return this.globalOperationHandlers.OrderBy(h => h.Rank);
		}
	}
}