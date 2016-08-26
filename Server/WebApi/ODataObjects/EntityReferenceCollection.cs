namespace ServiceApi.Server.WebApi.ODataObjects
{
	using System.Collections.Generic;
	using Microsoft.OData.Core.UriParser.Semantic;
	using ServiceApi.Model.Core.Serialization;

	public class EntityReferenceCollection : List<ODataPath>, IODataEntityReferenceCollection
	{
	}
}
