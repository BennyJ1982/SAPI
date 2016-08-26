// <copyright file="WritingEntityProcessorBase.cs" company="Facton GmbH">
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
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	public abstract class WritingEntityProcessorBase
	{
		private readonly IStructuralPropertyBinder structuralPropertyBinder;

		protected WritingEntityProcessorBase(IStructuralPropertyBinder structuralPropertyBinder)
		{
			this.structuralPropertyBinder = structuralPropertyBinder;
		}

		protected void SetPropertyValues(
			IBindableModelContext context,
			IEntitySet navigationRoot,
			IEntity targetEntity,
			ODataEntityDto oDataEntity)
		{
			this.SetPropertyValues(context, navigationRoot, targetEntity, oDataEntity, Enumerable.Empty<string>());
		}

		protected void SetPropertyValues(
			IBindableModelContext context,
			IEntitySet navigationRoot,
			IEntity targetEntity,
			ODataEntityDto oDataEntity,
			IEnumerable<string> propertiesToExclude)
		{
			var entityType = oDataEntity.GetEntityType(context);
			var relevantProperties = oDataEntity.Entry.Properties.Where(p => !propertiesToExclude.Contains(p.Name));
			foreach (var property in relevantProperties)
			{
				IStructuralProperty structuralProperty;
				if (entityType.TryGetStructuralProperty(property.Name, out structuralProperty))
				{
					if (!this.TryHandleStructuralProperty(targetEntity, oDataEntity, structuralProperty))
					{
						throw new InvalidOperationException("The included structural property \"" + property.Name + "\" could not be processed.");
					}

					continue;
				}

				INavigationProperty navigationProperty;
				if (entityType.TryGetNavigationProperty(property.Name, out navigationProperty))
				{
					if (!this.TryHandleNavigationProperty(context, targetEntity, oDataEntity, navigationProperty, navigationRoot))
					{
						throw new InvalidOperationException("The included navigation property \"" + property.Name + "\" could not be processed.");
					}

					continue;
				}

				throw new NotSupportedException("The included property \"" + property.Name + "\" is unsupported.");
			}
		}

		protected virtual bool TryHandleStructuralProperty(
			IEntity targetEntity,
			ODataEntityDto oDataEntity,
			IStructuralProperty structuralProperty)
		{
			var structuralEntityProperty = structuralProperty as IStructuralEntityProperty;
			if (structuralEntityProperty == null)
			{
				return false;
			}

			if (!this.structuralPropertyBinder.TrySetOnEntity(targetEntity, oDataEntity, structuralEntityProperty))
			{
				return false;
			}

			return true;
		}

		protected abstract bool TryHandleNavigationProperty(
			IBindableModelContext context,
			IEntity targetEntity,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot);
	}
}