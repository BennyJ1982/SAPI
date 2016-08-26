// <copyright file="StructuralPropertyBinder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	/// <summary>
	/// Sets structural properties on FACTON entities.
	/// </summary>
	public class StructuralPropertyBinder : IStructuralPropertyBinder
	{
		public bool TrySetOnEntity(IEntity targetEntity, ODataEntityDto sourceODataEntity, IStructuralEntityProperty property)
		{
			object value;
			if (!sourceODataEntity.TryGetPropertyValue(property, out value))
			{
				return false;
			}

			if (property.IsReadOnly(targetEntity))
			{
				return false;
			}

			return property.TrySetValueOnEntity(targetEntity, value);
		}
	}
}