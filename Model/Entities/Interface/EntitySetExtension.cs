// <copyright file="EntitySetExtension.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System;
	using System.Collections.Generic;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	public static class EntitySetExtension
	{
		public static IEntity CreateEntityInEntitySet(
			this IEntitySet entitySet,
			IBindableModelContext context,
			IDictionary<string, IDependency> dependencies)
		{
			IEntitySetBinding binding;
			if (!context.TryGetBinding(entitySet, out binding))
			{
				throw new ArgumentException("The specified entity set does not support the expected binding.", nameof(entitySet));
			}

			return binding.CreateAndAdd(dependencies);
		}
	}
}