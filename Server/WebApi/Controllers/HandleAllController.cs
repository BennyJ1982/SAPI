namespace ServiceApi.Server.WebApi.Controllers
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Web.Http;
	using System.Web.OData;
	using System.Web.OData.Extensions;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Edm;
	using ServiceApi.Model.Core.Serialization;
	using ServiceApi.Repository;
	using ServiceApi.Server.WebApi.Filters;
	using ServiceApi.Server.WebApi.ODataObjects;

	/// <summary>
	/// ODataController which handles all supported odata requests
	/// </summary>
	[StaticAuthentication]
	[EnableServiceApiFormatters]
	//[ControllerDebug]
	public class HandleAllController : ODataController
	{
		private readonly IODataRepository oDataRepository;

		public HandleAllController(IODataRepository oDataRepository)
		{
			this.oDataRepository = oDataRepository;
		}

		[HttpGet]
		public async Task<EdmEntityObjectCollection> GetEntityCollection(ODataPath path, QueryOptions query)
		{
			var result = await this.oDataRepository.Get(path, query);
			return result.ToCollection(this.Request.ODataProperties().Path.EdmType.As<IEdmCollectionType>());
		}

		[HttpGet]
		public async Task<IEdmEntityObject> GetEntity(ODataPath path, QueryOptions query)
		{
			var entity = (await this.oDataRepository.Get(path, query)).FirstOrDefault();
			if (entity == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			return entity.ToEdmEntityObject();
		}

		[HttpPost]
		public async Task<HttpResponseMessage> PostEntity(ServiceApiEdmEntityObject entity, ODataPath path, QueryOptions query)
		{
			HttpResponseMessage errorResponse;
			if (!this.TryValidateRequest(entity, path, this.oDataRepository.CanPost, out errorResponse))
			{
				return errorResponse;
			}

			var createdEntity = await this.oDataRepository.Post(path, query, entity.As<IODataEntityObject>());
			return this.Request.CreatePostResponse(createdEntity);
		}

		[HttpPatch]
		public async Task<HttpResponseMessage> PatchEntity(ServiceApiEdmEntityObject entity, ODataPath path, QueryOptions query)
		{
			HttpResponseMessage errorResponse;
			if (!this.TryValidateRequest(entity, path, this.oDataRepository.CanPatch, out errorResponse))
			{
				return errorResponse;
			}

			var patchedEntity = await this.oDataRepository.Patch(path, query, entity.As<IODataEntityObject>());
			return this.Request.CreatePatchResponse(patchedEntity);
		}

		private bool TryValidateRequest(IEdmObject entity, ODataPath path, Func<ODataPath, bool> canExecute, out HttpResponseMessage errorResponse)
		{
			if (entity == null)
			{
				errorResponse = this.Request.CreateBadRequestResponse();
				return false;
			}

			if (!canExecute(path))
			{
				errorResponse = this.Request.CreateMethodNotAllowedResponse();
				return false;
			}

			errorResponse = null;
			return true;
		}
	}
}
