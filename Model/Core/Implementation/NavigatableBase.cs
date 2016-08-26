﻿// <copyright file="NavigatableBase.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Microsoft.OData.Edm;

	public abstract class NavigatableBase : INavigatable
	{
		protected NavigatableBase(Operation supportedOperations)
		{
			this.SupportedOperations = supportedOperations;
		}

		public Operation SupportedOperations { get; }

		public IEntityType EntityType => this.GetTargetEntityType();

		public virtual IEdmEntityType EdmEntityType => this.GetTargetEntityType().ResultingEdmEntityType;

		protected abstract IEntityType GetTargetEntityType();
	}
}