// <copyright file="EntitySet.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Microsoft.OData.Edm;

	public class EntitySet : NavigatableBase, IEntitySet
	{
		private readonly IEntityType entityType;

		public EntitySet(IEdmNavigationSource resultingEdmType, IEntityType entityType, Operation supportedOperations)
			: base(supportedOperations)
		{
			this.Name = resultingEdmType.Name;
			this.entityType = entityType;
			this.ResultingEdmType = resultingEdmType;
		}

		public string Name { get; }

		protected override IEntityType GetTargetEntityType() => this.entityType;

		public IEdmNavigationSource ResultingEdmType { get; }
	}
}