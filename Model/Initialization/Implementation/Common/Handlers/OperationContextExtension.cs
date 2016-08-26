// <copyright file="OperationContextExtension.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.Common.Handlers
{
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Execution;
	using Facton.ServiceApi.Domain.Model.Entities.Serialization;

	public static class OperationContextExtension
	{
		// UNDONE BJ: This method is currently used to create the odata response of a POST or PATCH operation. It does simply render all properties.
		// Should be changed to respect the properties actually affected and to expand included navigation properties.
		public static ODataEntityDto CreatePatchOrPostResponse(
			this IOperationContext context,
			IEntity sourceEntity,
			IODataEntityDtoBuilderFactory dtoBuilderFactory)
		{
			var entityType = context.GetTargetEntityType();
			var dtoBuilder = dtoBuilderFactory.Create(entityType);

			dtoBuilder.ApplyAllStructuralPropertiesAndKeys(sourceEntity);
			return dtoBuilder.DtoUnderConstruction;
		}
	}
}