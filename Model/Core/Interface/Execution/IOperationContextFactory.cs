// <copyright file="IOperationContextFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using Facton.ServiceApi.Core;

	using Microsoft.OData.Core.UriParser.Semantic;

	public interface IOperationContextFactory
	{
		IOperationContext Create(IModelContext modelContext, Operation operation, ODataPath path, QueryOptions queryOptions);
	}
}