namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Queries;

	public interface IFactonQueryService
	{
		bool TryBuildQuery(string queryText, out IQuery query, out IEnumerable<string> errors);

		IEnumerable<IEntity> ExecuteFqlQuery(IQuery query);

		IEnumerable<IEntity> ExecuteFqlQuery(string queryText);
	}
}