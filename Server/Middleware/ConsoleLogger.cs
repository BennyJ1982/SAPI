namespace ServiceApi.Server.Middleware
{
	using System;
	using System.Diagnostics;
	using Microsoft.Owin.Logging;
	using ServiceApi.Model.Core.Serialization;

	class ConsoleLogger : ILogger, IMappingLogger
	{
		/// <summary>
		/// Aggregates most logging patterns to a single Operation.  This must be compatible with the Func representation in the OWIN environment.
		/// To check IsEnabled call WriteCore with only TraceEventType and check the return value, no event will be written.
		/// </summary>
		/// <param name="eventType"></param>
		/// <param name="eventId"></param>
		/// <param name="state"></param>
		/// <param name="exception"></param>
		/// <param name="formatter"></param>
		/// <returns></returns>
		public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
		{
			if (state != null)
			{
				var message = state.ToString();
				if (message.StartsWith("Executing:", StringComparison.InvariantCultureIgnoreCase))
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("Executing:");
					Console.ResetColor();

					message = message.Substring(10);
				}

				Console.WriteLine(message);
			}

			if (exception != null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(exception.ToString());
				Console.ResetColor();
			}

			return true;
		}

		public void Write(TraceEventType eventType, string message)
		{
			this.WriteCore(eventType, 0, message, null, null);
		}
	}
}
