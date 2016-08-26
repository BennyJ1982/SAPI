namespace Facton.ServiceApi.Domain.Model.Entities.Execution.Reading
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	using Microsoft.OData.Core.UriParser;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Core.UriParser.Visitors;
	using Microsoft.OData.Edm;

	public class EntityReadingPathSegmentHandler : PathSegmentHandler
	{
		private readonly IModelContext modelContext;

		private readonly ReadContext readContext;

		private INavigatable pendingEntityCollection;

		public EntityReadingPathSegmentHandler(IBindableModelContext modelContext)
		{
			this.modelContext = modelContext;
			this.readContext = new ReadContext(modelContext);
		}

		public EntityReadingPathSegmentHandler(IBindableModelContext modelContext, IEntity existingEntity)
		{
			this.modelContext = modelContext;
			this.readContext = new ReadContext(existingEntity.Enumerate(), modelContext);
		}

		public void HandlePath(IEnumerable<ODataPathSegment> pathSegment)
		{
			pathSegment.ForEach(item => item.HandleWith(this));
		}

		public IEnumerable<IEntity> GetResultingEntities()
		{
			if (this.pendingEntityCollection != null)
			{
				this.HandlePendingEntityCollection();
			}

			return this.readContext.Result;
		}

		public override void Handle(EntitySetSegment segment)
		{
			// we don't handle the entity set immediately but set it to pending as the next segment might be a key
			this.SetPendingEntityCollection(this.modelContext.GetEntitySetOrThrow(segment.EntitySet.Name));
		}

		public override void Handle(SingletonSegment segment)
		{
			EntityReadingHelper.ReadSingleton(this.readContext, this.modelContext.GetEntitySetOrThrow(segment.Singleton.Name));
		}

		public override void Handle(NavigationPropertySegment segment)
		{
			var sourceType = this.modelContext.GetEntityTypeOrThrow(segment.NavigationProperty.DeclaringType.As<IEdmEntityType>().Name);
			INavigationProperty navigationProperty;
			if (!sourceType.TryGetNavigationProperty(segment.NavigationProperty.Name, out navigationProperty))
			{
				throw new KeyNotFoundException("Navigation property " + segment.NavigationProperty.Name + " not found on type " + sourceType.Name);
			}

			if (navigationProperty.IsCollection())
			{
				// we don't handle the collection navigation property immediately but set it to pending as the next segment might be a key
				this.SetPendingEntityCollection(navigationProperty);
			}
			else
			{
				if (this.pendingEntityCollection != null)
				{
					this.HandlePendingEntityCollection();
				}

				EntityReadingHelper.ReadSingleNavigationProperty(this.readContext, navigationProperty);
			}
		}

		public override void Handle(KeySegment segment)
		{
			this.HandlePendingEntityCollection(segment.Keys);
		}

		private void SetPendingEntityCollection(INavigatable collection)
		{
			if (this.pendingEntityCollection != null)
			{
				this.HandlePendingEntityCollection();
			}

			var entitySet = collection as IEntitySet;
			if (entitySet == null)
			{
				if (!collection.As<INavigationProperty>().IsCollection())
				{
					throw new ArgumentException("The specified navigation source is not a collection.", nameof(collection));
				}
			}

			this.pendingEntityCollection = collection;
		}

		private void HandlePendingEntityCollection(IEnumerable<KeyValuePair<string, object>> keys)
		{
			var navigationProperty = this.pendingEntityCollection as INavigationProperty;
			if (navigationProperty != null)
			{
				var deserializedKeys = GetDeserializedKeys(keys, navigationProperty.TargetType);
				EntityReadingHelper.ReadCollectionNavigationProperty(this.readContext, navigationProperty, deserializedKeys);
			}
			else
			{
				var entitySet = this.pendingEntityCollection.As<IEntitySet>();
				var deserializedKeys = GetDeserializedKeys(keys, entitySet.EntityType);
				EntityReadingHelper.ReadEntitySet(this.readContext, entitySet, deserializedKeys);
			}

			this.pendingEntityCollection = null;
		}

		private void HandlePendingEntityCollection()
		{
			var navigationProperty = this.pendingEntityCollection as INavigationProperty;
			if (navigationProperty != null)
			{
				EntityReadingHelper.ReadCollectionNavigationProperty(this.readContext, navigationProperty);
			}
			else
			{
				EntityReadingHelper.ReadEntitySet(this.readContext, this.pendingEntityCollection.As<IEntitySet>());
			}

			this.pendingEntityCollection = null;
		}

		private static IEnumerable<KeyValuePair<string, object>> GetDeserializedKeys(
			IEnumerable<KeyValuePair<string, object>> keys,
			IEntityType entityType)
		{
			var deserializedKeys = new Dictionary<string, object>();
			foreach (var key in keys)
			{
				IStructuralProperty keyProperty;
				if (!entityType.TryGetKeyProperty(key.Key, out keyProperty))
				{
					throw new ODataUnrecognizedPathException("The specified key " + key.Key + " does not belong to the target entity type.");
				}

				deserializedKeys[key.Key] = keyProperty.DataType.As<IValueBasedDataType>().Deserialize(key.Value);
			}

			return deserializedKeys;
		}
	}
}