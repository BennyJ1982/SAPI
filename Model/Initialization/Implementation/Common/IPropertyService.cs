namespace Facton.ServiceApi.Domain.Model.Initialization.Common
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Metadata;

	/// <summary>
	/// Provides abstraction for accessing different kinds of facton properties.
	/// </summary>
	public interface IPropertyService : IMetadataItemProvider
	{
		IEnumerable<IProperty> AllRelevantProperties { get; }

		IProperty LabelProperty { get; }

		IProperty CompactSignatureProperty { get; }
	}
}
