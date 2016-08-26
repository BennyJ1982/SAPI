namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;

	public class PropertyService : IPropertyService
	{
		private readonly IMetadataService metadataService;

		public PropertyService(IMetadataService metadataService)
		{
			this.metadataService = metadataService;
			this.LabelProperty = this.metadataService.GetPropertyByName("Label");
			this.CompactSignatureProperty = this.metadataService.GetPropertyByName("_CompactSignature");
		}

		public IEnumerable<IProperty> AllRelevantProperties => this.metadataService.Properties.Where(IsRelevantProperty);

		public IProperty LabelProperty { get; }

		public IProperty CompactSignatureProperty { get; }

	
		public bool TryGetMetadataItem<TMetadataItem>(string itemName, out TMetadataItem metadataItem) where TMetadataItem : IMetadataItem
		{
			TMetadataItem item;
			if (!this.metadataService.TryGetMetadataItem(itemName, out item))
			{
				metadataItem = default(TMetadataItem);
				return false;
			}

			var property = item as IProperty;
			if (property != null && !IsRelevantProperty(property))
			{
				metadataItem = default(TMetadataItem);
				return false;
			}

			metadataItem = item;
			return true;
		}

		public bool TryGetMetadataItem<TMetadataItem>(IId itemId, out TMetadataItem metadataItem) where TMetadataItem : IMetadataItem
		{
			TMetadataItem item;
			if (!this.metadataService.TryGetMetadataItem(itemId, out item))
			{
				metadataItem = default(TMetadataItem);
				return false;
			}

			var property = item as IProperty;
			if (property != null && !IsRelevantProperty(property))
			{
				metadataItem = default(TMetadataItem);
				return false;
			}

			metadataItem = item;
			return true;
		}

		public IEnumerable<TMetadataItem> GetMetadataItems<TMetadataItem>() where TMetadataItem : IMetadataItem
		{
			return typeof(IProperty).IsAssignableFrom(typeof(TMetadataItem))
						? this.AllRelevantProperties.Cast<TMetadataItem>()
						: this.metadataService.GetMetadataItems<TMetadataItem>();
		}

		private static bool IsRelevantProperty(IProperty p)
		{
			return p.IsPublic && p.Name != FactonModelKeywords.IdPropertyName;
		}
	}
}