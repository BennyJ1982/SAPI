// <copyright file="IODataEntityDtoBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Serialization
{
	using System;
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;

	/// <summary>
	/// Builds an odata entity dto by applying structural and expanded navigation properties to it.
	/// </summary>
	public interface IODataEntityDtoBuilder
	{
		ODataEntityDto DtoUnderConstruction { get; }

		IEntityType EntityType { get; }

		void ApplyAllStructuralPropertiesAndKeys(IEntity sourceEntity);

		void ApplyNavigationProperty(
			INavigationProperty navigationProperty,
			IEnumerable<IEntity> entities,
			Action<IODataEntityDtoBuilder, IEntity> childDtoInitializer);

		void ApplyStructuralProperty(IStructuralEntityProperty property, IEntity sourceEntity);
	}
}