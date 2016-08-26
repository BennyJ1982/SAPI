namespace ServiceApi.Server.Middleware
{
	using Owin;

	/// <summary>
    /// Handy extension methods for using facton middleware
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Uses the FACTON logging middleware.
        /// </summary>
        public static void UseLogging(this IAppBuilder appBuilder)
        {
            appBuilder.Use<LoggingMiddleware>(appBuilder);
        }
    }
}
