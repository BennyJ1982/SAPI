namespace ServiceApi.Server.WebApi.Controllers
{
	using System;
	using System.Net.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Dispatcher;
	using ServiceApi.Repository;

	/// <summary>
	/// Simple controller activator that can inject to required dependecies into the HandleAllController.
	/// For other controllers, revert to default activator.
	/// </summary>
	internal class ControllerActivator : IHttpControllerActivator
	{
		private readonly IODataRepository oDataRepository;

		private readonly IHttpControllerActivator fallbackActivator;

		public ControllerActivator(IODataRepository oDataRepository, IHttpControllerActivator fallbackActivator)
		{
			this.oDataRepository = oDataRepository;
			this.fallbackActivator = fallbackActivator;
		}

		public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
		{
			if (controllerType == typeof(HandleAllController))
			{
				return new HandleAllController(this.oDataRepository);
			}

			return this.fallbackActivator.Create(request, controllerDescriptor, controllerType);
		}
	}
}
