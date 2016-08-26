// <copyright file="OperationContext.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using Facton.ServiceApi.Core;
	using Microsoft.OData.Core.UriParser.Semantic;

	public class OperationContext : IOperationContext
	{
		public OperationContext(
			ODataPath path,
			Operation operation,
			QueryOptions queryOptions,
			INavigatable navigationTarget,
			IEntitySet navigationRoot,
			IModelContext modelContext)
		{
			this.Path = path;
			this.Operation = operation;
			this.QueryOptions = queryOptions;
			this.NavigationTarget = navigationTarget;
			this.NavigationRoot = navigationRoot;
			this.ModelContext = modelContext;
		}


		public Operation Operation { get; }

		public INavigatable NavigationTarget { get; }

		public IEntitySet NavigationRoot { get; }

		public IModelContext ModelContext { get; }

		public QueryOptions QueryOptions { get; }

		public ODataPath Path { get; }
	}
}