// <copyright file="IMappingLogger.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Core
{
	using System.Diagnostics;

	/// <summary>
	/// Logger used in the mapping spike
	/// </summary>
	public interface IMappingLogger
	{
		void Write(TraceEventType eventType, string message);
	}
}