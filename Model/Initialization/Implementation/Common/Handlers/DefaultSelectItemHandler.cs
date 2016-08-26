namespace Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	using Microsoft.OData.Core.UriParser;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Core.UriParser.Visitors;

	/// <summary>
	/// Handles select and expand items specified in a GET query.
	/// </summary>
	public class DefaultSelectItemHandler : SelectItemHandler
	{
		private readonly IEntityReader entityReader;

		private readonly IODataEntityDtoBuilder resultingDtoBuilder;

		private readonly IEntity sourceEntity;

		private readonly IBindableModelContext modelContext;

		private readonly Action<IODataEntityDtoBuilder, IEntity> defaultChildDtoInitializer;

		public DefaultSelectItemHandler(
			IEntityReader entityReader,
			IBindableModelContext modelContext,
			IEntity sourceEntity,
			IODataEntityDtoBuilder resultingDtoBuilder,
			Action<IODataEntityDtoBuilder, IEntity> defaultChildDtoInitializer)
		{
			this.entityReader = entityReader;
			this.modelContext = modelContext;
			this.sourceEntity = sourceEntity;
			this.resultingDtoBuilder = resultingDtoBuilder;
			this.defaultChildDtoInitializer = defaultChildDtoInitializer;
		}

		public void HandleAll(IEnumerable<SelectItem> items)
		{
			items.ForEach(item => item.HandleWith(this));
		}

		public override void Handle(PathSelectItem item)
		{
			var propertySegment = item.SelectedPath.FirstSegment as PropertySegment;
			if (propertySegment == null)
			{
				return; // TODO: find out if we need to support this
			}

			IStructuralEntityProperty property;
			if (this.resultingDtoBuilder.EntityType.TryGetStructuralOrKeyProperty(propertySegment.Property.Name, out property))
			{
				this.resultingDtoBuilder.ApplyStructuralProperty(property, this.sourceEntity);
				return;
			}

			throw new ODataUnrecognizedPathException("The specified selected property could not be found");
		}

		public override void Handle(ExpandedNavigationSelectItem item)
		{
			var edmNavigationProperty = item.PathToNavigationProperty.LastSegment.As<NavigationPropertySegment>().NavigationProperty;
			INavigationProperty navigationProperty;
			if (!this.resultingDtoBuilder.EntityType.TryGetNavigationProperty(edmNavigationProperty.Name, out navigationProperty))
			{
				throw new KeyNotFoundException("Navigation property " + edmNavigationProperty.Name + " not found.");
			}

			var childEntities = this.entityReader.ReadEntitiesFromRelativePath(this.modelContext, this.sourceEntity, item.PathToNavigationProperty);

			if (item.SelectAndExpand == null || item.SelectAndExpand.AllSelected)
			{
				this.resultingDtoBuilder.ApplyNavigationProperty(navigationProperty, childEntities, this.defaultChildDtoInitializer);
			}
			else
			{
				// handle the select/expand clause of the child entity
				this.resultingDtoBuilder.ApplyNavigationProperty(
					navigationProperty,
					childEntities,
					(childDtoBuilder, childEntity) =>
						{
							var childHandler = new DefaultSelectItemHandler(
								this.entityReader,
								this.modelContext,
								childEntity,
								childDtoBuilder,
								this.defaultChildDtoInitializer);

							item.SelectAndExpand.SelectedItems.ForEach(i => i.HandleWith(childHandler));
						});
			}
		}
	}
}