namespace Facton.ServiceApi.Domain.Model.Core.Queries
{
	using System.Collections.Generic;
	using System.Text;

	public interface IQueryBuilder
	{
		StringBuilder FromBuilder { get; }

		StringBuilder SelectBuilder { get; }

		StringBuilder WhereBuilder { get; }

		IList<IQueryAttribute> Attributes { get; }

		string BuildQuery();
	}
}
