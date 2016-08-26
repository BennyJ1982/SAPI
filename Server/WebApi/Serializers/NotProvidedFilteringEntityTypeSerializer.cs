namespace ServiceApi.Server.WebApi.Serializers
{
	using System.Linq;
	using System.Web.OData;
	using System.Web.OData.Formatter.Serialization;
	using Microsoft.OData.Core;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Model.Core.Serialization.Annotations;

	public class NotProvidedFilteringEntityTypeSerializer : AnnotatingEntityTypeSerializer
	{
		public NotProvidedFilteringEntityTypeSerializer(ODataSerializerProvider serializerProvider)
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
				entry.Properties = entry.Properties.Where(p => !(p.Value == null && IsNotProvided(p, annotatableEntity))).ToArray();
			}

			return entry;
		}

		private static bool IsNotProvided(ODataProperty property, IAnnotatable annotatableEntity)
		{
			var nullValueAnnotation = annotatableEntity.GetAnnotationsByPropertyName<NullValueAnnotation>(property.Name).FirstOrDefault();
			if (nullValueAnnotation != null)
			{
				return nullValueAnnotation.AnnotationType == NullValueAnnotationType.NotProvided;
			}

			return false;
		}
	}
}