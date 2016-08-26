// <copyright file="PatchEntityOperationHandler.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	/// <summary>
	/// Handles PATCH requests for all FACTON entities.
	/// </summary>
	internal class PatchEntityOperationHandler : IOperationHandler
	{
		private readonly IEntityReader entityReader;

		private readonly IEntityUpdater entityUpdater;

		private readonly IODataEntityDtoBuilderFactory dtoBuilderFactory;

		public PatchEntityOperationHandler(IEntityReader entityReader, IEntityUpdater entityUpdater, IODataEntityDtoBuilderFactory dtoBuilderFactory)
		{
			this.entityReader = entityReader;
			this.entityUpdater = entityUpdater;
			this.dtoBuilderFactory = dtoBuilderFactory;
		}

		public int Rank => (int)OperationHandlerRanks.Default;

		public IEnumerable<ODataEntityDto> Handle(IOperationContext context, ODataEntityDto incomingODataEntity)
		{
			var modelContext = context.ModelContext.As<IBindableModelContext>();

			var entityToPatch = this.entityReader.ReadEntitiesFromPath(modelContext, context.Path).SingleOrDefault();
			if (entityToPatch == null)
			{
				throw new KeyNotFoundException("The specified entity could not be found.");
			}

			this.entityUpdater.UpdateEntity(modelContext, context.NavigationRoot, entityToPatch, incomingODataEntity);
			return context.CreatePatchOrPostResponse(entityToPatch, this.dtoBuilderFactory).Enumerate();
		}

		public bool CanHandle(IOperationContext context)
		{
			return context.Operation == Operation.Patch && context.ModelContext is IBindableModelContext;
		}
	}
}