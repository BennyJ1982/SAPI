// <copyright file="IEntityUpdater.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;

	/// <summary>
	/// Contains the general logic for updating an entity from an odata request, while respecting child entities included as reference links.
	/// </summary>
	public interface IEntityUpdater
	{
		void UpdateEntity(IBindableModelContext context, IEntitySet navigationRoot, IEntity targetEntity, ODataEntityDto oDataEntity);
	}
}