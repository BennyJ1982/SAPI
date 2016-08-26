// <copyright file="UncontainedNavigationPropertyParser.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	public class UncontainedNavigationPropertyParser : IUncontainedNavigationPropertyParser
	{
		private readonly IReferenceParser referenceParser;

		private Func<IBindableModelContext, INavigationProperty, IEntitySet, IEnumerable<ODataEntityDto>, IEnumerable<IEntity>>
			uncontainedEntitiesFactory;

		public UncontainedNavigationPropertyParser(IReferenceParser referenceParser)
		{
			this.referenceParser = referenceParser;
		}

		public bool TryGetLinkedOrInlineEntities(
			IBindableModelContext context,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			out IEnumerable<IEntity> entities)
		{
			if (navigationProperty.IsContained())
			{
				throw new ArgumentException("Navigation property must be uncontained.", nameof(navigationProperty));
			}

			// parse reference links as well as inline entities (both can be included at the same time)
			var resultingEntities = new List<IEntity>();
			var success = this.TryParseReferencedEntities(context, oDataEntity, navigationProperty, resultingEntities)
						| this.TryParseAndCreateInlineEntities(context, oDataEntity, navigationProperty, navigationRoot, resultingEntities);

			if (success)
			{
				entities = resultingEntities;
				return true;
			}

			entities = null;
			return false;
		}

		public bool TryGetLinkedEntities(
			IBindableModelContext context,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			out IEnumerable<IEntity> entities)
		{
			if (navigationProperty.IsContained())
			{
				throw new ArgumentException("Navigation property must be uncontained.", nameof(navigationProperty));
			}

			var resultingEntities = new List<IEntity>();
			if (this.TryParseReferencedEntities(context, oDataEntity, navigationProperty, resultingEntities))
			{
				entities = resultingEntities;
				return true;
			}

			entities = null;
			return false;
		}

		private bool TryParseAndCreateInlineEntities(
			IBindableModelContext context,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			List<IEntity> resultingEntities)
		{
			IEnumerable<ODataEntityDto> inlineODataEntities;
			if (oDataEntity.TryGetInlineODataEntities(navigationProperty.Name, out inlineODataEntities))
			{
				if (this.uncontainedEntitiesFactory == null)
				{
					throw new InvalidOperationException("Cannot create uncontained entities. Factory hasn't been set.");
				}

				var createdEntities = this.uncontainedEntitiesFactory(context, navigationProperty, navigationRoot, inlineODataEntities);
				resultingEntities.AddRange(createdEntities);
				return true;
			}

			return false;
		}

		private bool TryParseReferencedEntities(
			IBindableModelContext context,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			List<IEntity> resultingEntities)
		{
			IEnumerable<IEntity> referencedEntities;
			if (this.referenceParser.TryParseReferencedEntities(context, navigationProperty, oDataEntity, out referencedEntities))
			{
				resultingEntities.AddRange(referencedEntities);
				return true;
			}

			return false;
		}

		public void SetUncontainedEntitiesFactory(
			Func<IBindableModelContext, INavigationProperty, IEntitySet, IEnumerable<ODataEntityDto>, IEnumerable<IEntity>> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException(nameof(factory));
			}

			if (this.uncontainedEntitiesFactory != null)
			{
				throw new InvalidOperationException("Uncontained entities factory already set");
			}

			this.uncontainedEntitiesFactory = factory;
		}
	}
}