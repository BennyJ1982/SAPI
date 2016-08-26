// <copyright file="IUncontainedNavigationPropertyParser.cs" company="Facton GmbH">
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

	/// <summary>
	/// Parses uncontained navigation properties containing either reference links or inline odata entities.
	/// </summary>
	public interface IUncontainedNavigationPropertyParser
	{
		bool TryGetLinkedOrInlineEntities(
			IBindableModelContext context,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			out IEnumerable<IEntity> entities);

		bool TryGetLinkedEntities(
			IBindableModelContext context,
			ODataEntityDto oDataEntity,
			INavigationProperty navigationProperty,
			out IEnumerable<IEntity> entities);
	}
}