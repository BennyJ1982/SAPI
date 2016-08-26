// <copyright file="IStructuralPropertyBinder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;

	public interface IStructuralPropertyBinder
	{
		bool TrySetOnEntity(IEntity targetEntity, ODataEntityDto sourceODataEntity, IStructuralEntityProperty property);
	}
}