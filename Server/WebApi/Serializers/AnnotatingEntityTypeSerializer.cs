namespace ServiceApi.Server.WebApi.Serializers
{
	using System.Linq;
	using System.Web.OData;
	using System.Web.OData.Formatter.Serialization;
	using Microsoft.OData.Core;
	using ServiceApi.Model.Core.Serialization;

	public class AnnotatingEntityTypeSerializer : ODataEntityTypeSerializer
	{

		protected AnnotatingEntityTypeSerializer(ODataSerializerProvider serializerProvider)
			: base(serializerProvider)
		{
		}

		public override ODataEntry CreateEntry(SelectExpandNode selectExpandNode, EntityInstanceContext entityInstanceContext)
		{
			var entry = base.CreateEntry(selectExpandNode, entityInstanceContext);

			// get backing edm entity object in order to access annonations
			var annotatableEntity = entityInstanceContext.EdmObject as IAnnotatable;
			if (annotatableEntity != null)
			{
				foreach (var propertyName in annotatableEntity.AnnotatedPropertyNames)
				{
					var property = entry.Properties.First(p => p.Name == propertyName);
					foreach (var annotation in annotatableEntity.GetAnnotationsByPropertyName(propertyName))
					{
						property.InstanceAnnotations.Add(new ODataInstanceAnnotation(annotation.Name, new ODataPrimitiveValue(annotation.Value)));
					}
				}
			}

			return entry;
		}
	}
}