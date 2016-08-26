// <copyright file="FactonQueryService.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

	using Facton.Infrastructure.Entities;
	using Facton.Infrastructure.Entities.Queries;
	using Facton.Infrastructure.Queries;
	using Facton.ServiceApi.Core;

	using QueryOptions = Facton.Infrastructure.Entities.Queries.QueryOptions;

	public class FactonQueryService : IFactonQueryService
	{
		private const int MaxRows = 50; // Hard row limit for the Spike (to avoid long running queries)

		private readonly IQueryService queryService;
		private readonly IEntityQueryService entityQueryService;
		private readonly IMappingLogger logger;

		public FactonQueryService(
			IQueryService queryService,
			IEntityQueryService entityQueryService,
			IMappingLogger logger)
		{
			this.queryService = queryService;
			this.entityQueryService = entityQueryService;
			this.logger = logger;
		}

		public bool TryBuildQuery(string queryText, out IQuery query, out IEnumerable<string> errors)
		{
			IQueryBuilder builder;
			IList<string> compileErrors;

			if (!this.queryService.TryCompile(queryText, out builder, out compileErrors))
			{
				query = null;
				errors = compileErrors;
				return false;
			}

			if (!builder.TryBuildQuery(out query, out compileErrors))
			{
				errors = compileErrors;
				return false;
			}

			errors = new List<string>();
			return true;
		}

		public IEnumerable<IEntity> ExecuteFqlQuery(IQuery query)
		{
			this.logger.Write(TraceEventType.Information, "Executing: " + query.Fql);
			var queryResult = this.entityQueryService.ReadByQuery(query, QueryOptions.Default);
			if (queryResult.State != QueryResultState.Completed)
			{
				return Enumerable.Empty<IEntity>();
			}

			return ReadLimited(queryResult.Result);
		}

		public IEnumerable<IEntity> ExecuteFqlQuery(string queryText)
		{
			IQuery query;
			IEnumerable<string> errors;
			if (!this.TryBuildQuery(queryText, out query, out errors))
			{
				throw new ArgumentException("Query does not compile.", nameof(queryText));
			}

			return this.ExecuteFqlQuery(query);
		}

		/// <summary>
		/// Reads only as many entities as the maximum permits.
		/// </summary>
		private static IEnumerable<IEntity> ReadLimited(IEnumerable<IEntity> entities)
		{
			var entitiesRead = 0;
			foreach (var entity in entities)
			{
				yield return entity;
				if (++entitiesRead == MaxRows)
				{
					break;
				}
			}
		}
	}
}