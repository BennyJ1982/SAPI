namespace ServiceApi.Server.WebApi
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Web.OData.Extensions;
	using Microsoft.OData.Core.UriParser;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Server.WebApi.ODataObjects;

	public static class HttpRequestMessageExtensions
	{
		public static HttpResponseMessage CreateBadRequestResponse(this HttpRequestMessage request)
		{
			return request.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				"The request did not contain an entity that's valid in the given context.");
		}

		public static HttpResponseMessage CreateMethodNotAllowedResponse(this HttpRequestMessage request)
		{
			return request.CreateErrorResponse(
				HttpStatusCode.MethodNotAllowed,
				"The requested action cannot be performed against the given entity set in the given context.");
		}

		public static HttpResponseMessage CreatePatchResponse(this HttpRequestMessage request, IODataEntityObject patchedEntity)
		{
			return request.CreateResponse(HttpStatusCode.OK, patchedEntity.ToEdmEntityObject());
		}

		public static HttpResponseMessage CreatePostResponse(this HttpRequestMessage request, IODataEntityObject createdEntity)
		{
			return request.CreateResponse(HttpStatusCode.Created, createdEntity.ToEdmEntityObject());
		}

		/// <summary>
		/// Gets a ODataUriParser from Microsoft.OData.Core from the provided HTTP request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The ODataUriParser.</returns>
		public static ODataUriParser GetODataUriParser(this HttpRequestMessage request)
		{
			return request.GetODataUriParser(request.RequestUri);
		}

		/// <summary>
		/// Gets a ODataUriParser from Microsoft.OData.Core from the provided HTTP request and Uri.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="uri">The URI, either absolute or relative to the service root.</param>
		/// <returns>The ODataUriParser.</returns>
		public static ODataUriParser GetODataUriParser(this HttpRequestMessage request, Uri uri)
		{
			var oDataProperties = request.ODataProperties();

			if (uri.IsAbsoluteUri)
			{
				var requestUriString = request.RequestUri.ToString();
				var odataPathString=oDataProperties.Path.ToString();
				var serviceRoot = requestUriString.Substring(0, requestUriString.IndexOf(odataPathString, StringComparison.InvariantCulture));

				return new ODataUriParser(oDataProperties.Model, new Uri(serviceRoot), uri);
			}

			return new ODataUriParser(oDataProperties.Model, uri);
		}
	}
}
