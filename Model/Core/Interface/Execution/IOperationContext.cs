// <copyright file="IOperationContext.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using Facton.ServiceApi.Core;

	using Microsoft.OData.Core.UriParser.Semantic;

	/// <summary>
	/// Contains information about a CRUD (Change, Return, Update Delete) operation 
	/// requested from an entity type and offers a way to create DTO response objects.
	/// </summary>
	public interface IOperationContext
	{
		Operation Operation { get; }

		/// <summary>
		/// Gets the last navigatable element of the path this context is based on. Determines the resulting entity type.
		/// </summary>
		INavigatable NavigationTarget { get; }

		/// <summary>
		/// Gets the first navigatable element (always an entity set) of the path this context is based on.
		/// </summary>
		IEntitySet NavigationRoot { get; }

		IModelContext ModelContext { get; }

		QueryOptions QueryOptions { get; }

		ODataPath Path { get; }
	}
}