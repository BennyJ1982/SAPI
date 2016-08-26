// <copyright file="QueryOptions.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Core
{
	using Microsoft.OData.Core.UriParser.Semantic;

	/// <summary>
	/// Holder for the parsed OData query options.
	/// </summary>
	public class QueryOptions
	{
		public SelectExpandClause SelectExpandClause { get; set; }

		public FilterClause FilterClause { get; set; }

		public OrderByClause OrderByClause { get; set; }

		public SearchClause SearchClause { get; set; }

		public long? Top { get; set; }

		public long? Skip { get; set; }
	}
}