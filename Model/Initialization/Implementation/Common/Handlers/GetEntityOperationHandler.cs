// <copyright file="GetEntityOperationHandler.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	using Microsoft.OData.Core.UriParser.Semantic;

	/// <summary>
	/// Handles a GET operation for FACTON entities
	/// </summary>
	public class GetEntityOperationHandler : IOperationHandler
	{
		private readonly IEntityReader entityReader;

		private readonly IODataEntityDtoBuilderFactory dtoBuilderFactory;

		public GetEntityOperationHandler(IEntityReader entityReader, IODataEntityDtoBuilderFactory dtoBuilderFactory)
		{
			this.entityReader = entityReader;
			this.dtoBuilderFactory = dtoBuilderFactory;
		}

		public int Rank => (int)OperationHandlerRanks.Default;

		public IEnumerable<ODataEntityDto> Handle(IOperationContext context, ODataEntityDto incomingODataEntity)
		{
			var entities = this.entityReader.ReadEntitiesFromPath(context.ModelContext.As<IBindableModelContext>(), context.Path);

			return this.SerializeEntities(
				context.ModelContext.As<IBindableModelContext>(),
				entities,
				context.GetTargetEntityType(),
				context.QueryOptions.SelectExpandClause);
		}

		public bool CanHandle(IOperationContext context)
		{
			return context.Operation == Operation.Get && context.ModelContext is IBindableModelContext;
		}

		private IEnumerable<ODataEntityDto> SerializeEntities(
			IBindableModelContext context,
			IEnumerable<IEntity> entities,
			IEntityType entityType,
			SelectExpandClause selectExpandClause)
		{
			foreach (var entity in entities)
			{
				var dtoBuilder = this.dtoBuilderFactory.Create(entityType);
				if (selectExpandClause == null || selectExpandClause.AllSelected)
				{
					SelectAllProperties(dtoBuilder, entity);
				}
				else
				{
					var selectItemHandler = new DefaultSelectItemHandler(this.entityReader, context, entity, dtoBuilder, SelectAllProperties);
					selectItemHandler.HandleAll(selectExpandClause.SelectedItems);
				}

				yield return dtoBuilder.DtoUnderConstruction;
			}
		}

		private static void SelectAllProperties(IODataEntityDtoBuilder dtoBuilder, IEntity sourceEntity)
		{
			dtoBuilder.ApplyAllStructuralPropertiesAndKeys(sourceEntity);
		}
	}
}