// <copyright file="EntityUpdater.cs" company="Facton GmbH">
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

	public class EntityUpdater : WritingEntityProcessorBase, IEntityUpdater
	{
		private readonly INavigationPropertyBinder navigationPropertyBinder;

		private readonly IUncontainedNavigationPropertyParser navigationPropertyParser;

		public EntityUpdater(
			IStructuralPropertyBinder structuralPropertyBinder,
			INavigationPropertyBinder navigationPropertyBinder,
			IUncontainedNavigationPropertyParser navigationPropertyParser)
			: base(structuralPropertyBinder)
		{
			this.navigationPropertyBinder = navigationPropertyBinder;
			this.navigationPropertyParser = navigationPropertyParser;
		}

		public void UpdateEntity(IBindableModelContext context, IEntitySet navigationRoot, IEntity targetEntity, ODataEntityDto oDataEntity)
		{
			this.SetPropertyValues(context, navigationRoot, targetEntity, oDataEntity);
		}

		protected override bool TryHandleStructuralProperty(
			IEntity targetEntity,
			ODataEntityDto oDataEntity,
			IStructuralProperty structuralProperty)
		{
			var structuralEntityProperty = structuralProperty as IStructuralEntityProperty;
			if (structuralEntityProperty != null && structuralEntityProperty.IsReadOnly(targetEntity))
			{
				// According to OData v4 Part 1 (Protocol), section 11.4.3 and 11.4.4, properties which are non-updatable should be ignored
				// during Update and Upsert. Hence, we return true here, claiming the property was handled correctly.
				return true;
			}

			return base.TryHandleStructuralProperty(targetEntity, oDataEntity, structuralProperty);
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
				throw new InvalidOperationException("Only uncontained navigation properties can be processed while updating the parent.");
			}

			IEnumerable<IEntity> entities;
			if (this.navigationPropertyParser.TryGetLinkedEntities(context, oDataEntity, navigationProperty, out entities))
			{
				this.navigationPropertyBinder.SetUncontainedNavigationProperty(context, targetEntity, navigationProperty, entities);
				return true;
			}

			return false;
		}
	}
}