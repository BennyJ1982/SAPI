// <copyright file="QueryBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Queries
{
	using System.Collections.Generic;
	using System.Text;

	public class QueryBuilder : IQueryBuilder
	{
		public QueryBuilder()
		{
			this.FromBuilder = new StringBuilder();
			this.SelectBuilder = new StringBuilder();
			this.WhereBuilder = new StringBuilder();
			this.Attributes = new List<IQueryAttribute>();
		}

		public StringBuilder FromBuilder { get; }

		public StringBuilder SelectBuilder { get; }

		public StringBuilder WhereBuilder { get; }

		public IList<IQueryAttribute> Attributes { get; }

		public string BuildQuery()
		{
			var builder = new StringBuilder();

			if (this.SelectBuilder.Length > 0)
			{
				builder.Append("SELECT ");
				builder.Append(this.SelectBuilder);
				builder.Append(" ");
			}

			if (this.FromBuilder.Length > 0)
			{
				builder.Append("FROM ");
				builder.Append(this.FromBuilder);
				builder.Append(" ");
			}

			if (this.WhereBuilder.Length > 0)
			{
				builder.Append("WHERE ");
				builder.Append(this.WhereBuilder);
			}

			return builder.ToString();
		}
	}
}