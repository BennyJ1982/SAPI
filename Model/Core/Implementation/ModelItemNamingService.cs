// <copyright file="ModelItemNamingService.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using System.Linq;
	using Facton.Infrastructure.Metadata;

	public class ModelItemNamingService : IModelItemNamingService
	{
		private static readonly char[] ForbiddenPropertyCharacters = { ':', '-' };

		public string GetSafeEdmPropertyName(IProperty property)
		{
			return ForbiddenPropertyCharacters.Aggregate(property.Name, (current, forbiddenCharacter) => current.Replace(forbiddenCharacter, '_'));
		}
	}
}