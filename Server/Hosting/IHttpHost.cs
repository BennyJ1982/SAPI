namespace ServiceApi.Server.Hosting
{
	using System;

	public interface IHttpHost : IDisposable
	{
		void Open(string baseUrl);
	}
}