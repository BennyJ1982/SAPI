// <copyright file="IReferenceParser.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;

	public interface IReferenceParser
	{
		bool TryParseReferencedEntities(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			ODataEntityDto oDataEntity,
			out IEnumerable<IEntity> entities);
	}
}