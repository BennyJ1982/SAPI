// <copyright file="NavigationProperty.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using System;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Microsoft.OData.Edm;

	public class NavigationProperty : NavigatableBase, INavigationProperty
	{
		public NavigationProperty(IEdmNavigationProperty resultingEdmType, IEntityType targetType, Operation supportedOperations)
			: base(supportedOperations)
		{
			if (resultingEdmType == null)
			{
				throw new ArgumentNullException(nameof(resultingEdmType));
			}

			if (targetType == null)
			{
				throw new ArgumentNullException(nameof(targetType));
			}

			this.ResultingEdmType = resultingEdmType;
			this.TargetType = targetType;
		}

		public IEdmNavigationProperty ResultingEdmType { get; }

		public IEntityType TargetType { get; }

		public string Name => this.ResultingEdmType.Name;

		protected override IEntityType GetTargetEntityType() => this.TargetType;
	}
}