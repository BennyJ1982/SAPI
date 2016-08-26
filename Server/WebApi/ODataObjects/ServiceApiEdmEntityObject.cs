namespace ServiceApi.Server.WebApi.ODataObjects
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.OData;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Edm;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Model.Core.Serialization.Annotations;

	/// <summary>
	/// Custom EdmEntityObject which implements IODataEntityObject. Used everywhere in the Service API.
	/// </summary>
	public class ServiceApiEdmEntityObject : EdmEntityObject, IODataEntityObject
	{
		private readonly IDictionary<string, List<IAnnotation>> propertyAnnotations = new Dictionary<string, List<IAnnotation>>();

		public ServiceApiEdmEntityObject(IEdmEntityType edmType)
			: this(edmType, isNullable: false)
		{
		}

		public ServiceApiEdmEntityObject(IEdmEntityTypeReference edmType)
			: this(edmType.EntityDefinition(), edmType.IsNullable)
		{
		}

		public ServiceApiEdmEntityObject(IEdmEntityType edmType, bool isNullable)
			: base(edmType, isNullable)
		{
		}

		public IEnumerable<string> AnnotatedPropertyNames => this.propertyAnnotations.Keys;

		public IEnumerable<IAnnotation> GetAnnotationsByPropertyName(string propertyName)
		{
			List<IAnnotation> annotations;
			if (this.propertyAnnotations.TryGetValue(propertyName, out annotations))
			{
				return annotations;
			}

			return Enumerable.Empty<IAnnotation>();
		}

		public void AddPropertyAnnotation(string propertyName, IAnnotation annotation)
		{
			List<IAnnotation> annotations;
			if (!this.propertyAnnotations.TryGetValue(propertyName, out annotations))
			{
				this.propertyAnnotations[propertyName] = annotations = new List<IAnnotation>();
			}

			annotations.Add(annotation);
		}

		public bool TryGetPropertyValue<T>(string propertyName, out T value)
		{
			return this.As<EdmStructuredObject>().TryGetPropertyValue<T>(propertyName, out value);
		}

		public bool TrySetPropertyValue<T>(string propertyName, T value)
		{
			return this.As<EdmStructuredObject>().TrySetPropertyValue<T>(propertyName, value);
		}
	}
}

