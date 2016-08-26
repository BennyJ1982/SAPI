// <copyright file="EntityCreator.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	public class EntityCreator : WritingEntityProcessorBase, IEntityCreator
	{
		private readonly INavigationPropertyBinder navigationPropertyBinder;

		private readonly IUncontainedNavigationPropertyParser navigationPropertyParser;

		private readonly IDependencyResolver dependencyResolver;

		public EntityCreator(
			IStructuralPropertyBinder structuralPropertyBinder,
			INavigationPropertyBinder navigationPropertyBinder,
			IUncontainedNavigationPropertyParser navigationPropertyParser,
			IDependencyResolver dependencyResolver)
			: base(structuralPropertyBinder)
		{
			this.navigationPropertyBinder = navigationPropertyBinder;
			this.navigationPropertyParser = navigationPropertyParser;
			this.dependencyResolver = dependencyResolver;
		}

		public IEntity CreateInEntitySet(IBindableModelContext context, IEntitySet entitySet, ODataEntityDto oDataEntity)
		{
			var dependencies = this.dependencyResolver.ResolveDependencies(context, entitySet, entitySet, oDataEntity);
			var createdEntity = entitySet.CreateEntityInEntitySet(context, dependencies);

			this.SetPropertyValues(context, entitySet, createdEntity, oDataEntity, dependencies.Keys);
			return createdEntity;
		}

		public IEntity CreateInContainedNavigationProperty(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			IEntity parentEntity,
			ODataEntityDto oDataEntity)
		{
			var dependencies = this.dependencyResolver.ResolveDependencies(context, navigationProperty, navigationRoot, oDataEntity);
			var createdEntity = navigationProperty.CreateEntityInContainedNavigationProperty(context, parentEntity, dependencies);

			this.SetPropertyValues(context, navigationRoot, createdEntity, oDataEntity, dependencies.Keys);
			return createdEntity;
		}

		public IEnumerable<IEntity> CreateInContainedNavigationProperty(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			IEntity parentEntity,
			IEnumerable<ODataEntityDto> oDataEntities)
		{
			var createdEntities = new List<IEntity>();
			foreach (var oDataEntity in oDataEntities)
			{
				createdEntities.Add(this.CreateInContainedNavigationProperty(context, navigationProperty, navigationRoot, parentEntity, oDataEntity));
			}

			return createdEntities;
		}

		public IEnumerable<IEntity> CreateInUncontainedNavigationProperty(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			IEnumerable<ODataEntityDto> oDataEntities)
		{
			IEntitySet targetEntitySet;
			if (navigationProperty.TryGetNavigationPropertyTarget(context, navigationRoot, out targetEntitySet)
				&& targetEntitySet.SupportedOperations.HasFlag(Operation.Post))
			{
				return oDataEntities.Select(oDataEntity => this.CreateInEntitySet(context, targetEntitySet, oDataEntity)).ToArray();
			}

			throw new InvalidOperationException(
				"No valid navigation target found for property " + navigationProperty.Name + " in the context of " + navigationRoot.Name + ".");
		}

		protected override bool TryHandleNavigationProperty(
			IBindableModelContext context,
			IEntity targetEntity,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot)
		{
			if (navigationProperty.IsContained())
			{
				IEnumerable<ODataEntityDto> inlineODataEntities;
				if (!oDataEntity.TryGetInlineODataEntities(navigationProperty.Name, out inlineODataEntities))
				{
					return false;
				}

				this.CreateInContainedNavigationProperty(context, navigationProperty, navigationRoot, targetEntity, inlineODataEntities);
				return true;
			}

			IEnumerable<IEntity> entities;
			if (this.navigationPropertyParser.TryGetLinkedOrInlineEntities(context, oDataEntity, navigationProperty, navigationRoot, out entities))
			{
				this.navigationPropertyBinder.SetUncontainedNavigationProperty(context, targetEntity, navigationProperty, entities);
				return true;
			}

			return false;
		}
	}
}