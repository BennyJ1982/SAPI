// <copyright file="OperationContextExtension.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;

	public static class OperationContextExtension
	{
		public static IEntityType GetTargetEntityType(this IOperationContext context)
		{
			return context.NavigationTarget.EntityType;
		}
	}
}