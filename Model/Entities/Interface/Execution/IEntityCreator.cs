// <copyright file="IEntityCreator.cs" company="Facton GmbH">
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
	/// Contains the general logic for creating an entity from an odata request, while respecting child entities included
	/// as inline content or reference links, as well as resolving dependencies. Uses the bindings of the affected entity sets 
	/// or navigation properties to perform the create.
	/// </summary>
	public interface IEntityCreator
	{
		IEntity CreateInEntitySet(
			IBindableModelContext context,
			IEntitySet entitySet,
			ODataEntityDto oDataEntity);

		IEntity CreateInContainedNavigationProperty(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			IEntity parentEntity,
			ODataEntityDto oDataEntity);

		IEnumerable<IEntity> CreateInContainedNavigationProperty(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			IEntity parentEntity,
			IEnumerable<ODataEntityDto> oDataEntities);

		IEnumerable<IEntity> CreateInUncontainedNavigationProperty(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			IEntitySet navigationRoot,
			IEnumerable<ODataEntityDto> oDataEntities);
	}
}