// <copyright file="PostEntityOperationHandler.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	/// <summary>
	/// The default handler for posting FACTON entities which may include associated child entities or links to those.
	/// </summary>
	public class PostEntityOperationHandler : IOperationHandler
	{
		private readonly IEntityReader entityReader;

		private readonly IEntityCreator entityCreator;

		private readonly IODataEntityDtoBuilderFactory dtoBuilderFactory;

		public PostEntityOperationHandler(IEntityReader entityReader, IEntityCreator entityCreator, IODataEntityDtoBuilderFactory dtoBuilderFactory)
		{
			this.entityReader = entityReader;
			this.entityCreator = entityCreator;
			this.dtoBuilderFactory = dtoBuilderFactory;
		}

		public int Rank => (int)OperationHandlerRanks.Default;

		public IEnumerable<ODataEntityDto> Handle(IOperationContext context, ODataEntityDto incomingODataEntity)
		{
			IEntity createdEntity;
			if (this.TryHandlePostInContainedNavigationProperty(context, incomingODataEntity, out createdEntity))
			{
				return context.CreatePatchOrPostResponse(createdEntity, this.dtoBuilderFactory).Enumerate();
			}

			if (this.TryHandlePostInEntitySet(context, incomingODataEntity, out createdEntity))
			{
				return context.CreatePatchOrPostResponse(createdEntity, this.dtoBuilderFactory).Enumerate();
			}

			throw new InvalidOperationException("The POST operation cannot be performed against the specified URL.");
		}

		public bool CanHandle(IOperationContext context)
		{
			return context.Operation == Operation.Post && context.ModelContext is IBindableModelContext;
		}

		private bool TryHandlePostInEntitySet(IOperationContext context, ODataEntityDto oDataEntity, out IEntity createdEntity)
		{
			var entitySet = context.NavigationTarget as IEntitySet;
			if (entitySet == null)
			{
				createdEntity = null;
				return false;
			}

			var bindableModelContext = context.ModelContext.As<IBindableModelContext>();
			createdEntity = this.entityCreator.CreateInEntitySet(bindableModelContext, context.NavigationTarget.As<IEntitySet>(), oDataEntity);
			return true;
		}

		private bool TryHandlePostInContainedNavigationProperty(
			IOperationContext context,
			ODataEntityDto oDataEntity,
			out IEntity createdEntity)
		{
			var navigationProperty = context.NavigationTarget as INavigationProperty;
			if (navigationProperty == null || !navigationProperty.IsContained())
			{
				createdEntity = null;
				return false;
			}

			var bindableModelContext = context.ModelContext.As<IBindableModelContext>();
			var parentPath = context.Path.Take(context.Path.Count - 1);
			var parentEntity = this.entityReader.ReadEntitiesFromPath(bindableModelContext, parentPath).SingleOrDefault();
			if (parentEntity == null)
			{
				throw new KeyNotFoundException("The preceeding entity could not be found.");
			}

			createdEntity = this.entityCreator.CreateInContainedNavigationProperty(
				bindableModelContext,
				navigationProperty,
				context.NavigationRoot,
				parentEntity,
				oDataEntity);

			return true;
		}
	}
}