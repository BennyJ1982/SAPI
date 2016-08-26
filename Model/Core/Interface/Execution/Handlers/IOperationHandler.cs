// <copyright file="IOperationHandler.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution.Handlers
{
	using System.Collections.Generic;

	using Facton.ServiceApi.Core;

	public interface IOperationHandler
	{
		int Rank { get; }

		IEnumerable<ODataEntityDto> Handle(IOperationContext context, ODataEntityDto incomingODataEntity);

		bool CanHandle(IOperationContext context);
	}
}