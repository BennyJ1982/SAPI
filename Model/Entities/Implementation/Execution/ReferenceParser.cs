// <copyright file="ReferenceParser.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities.Execution
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Core;
	using Facton.ServiceApi.Domain.Model.Core;

	using Microsoft.OData.Core.UriParser.Semantic;

	public class ReferenceParser : IReferenceParser
	{
		private readonly IEntityReader entityReader;

		public ReferenceParser(IEntityReader entityReader)
		{
			this.entityReader = entityReader;
		}

		public bool TryParseReferencedEntities(
			IBindableModelContext context,
			INavigationProperty navigationProperty,
			ODataEntityDto oDataEntity,
			out IEnumerable<IEntity> entities)
		{
			IEnumerable<ODataPath> referenceLinks;
			if (!TryGetEntityReferenceLinks(oDataEntity, navigationProperty.Name, out referenceLinks))
			{
				entities = null;
				return false;
			}

			var readEntities = new List<IEntity>();
			foreach (var referenceLink in referenceLinks)
			{
				readEntities.AddRange(this.entityReader.ReadEntitiesFromPath(context, referenceLink));
			}

			entities = readEntities;
			return true;
		}

		private static bool TryGetEntityReferenceLinks(
			ODataEntityDto oDataEntity,
			string navigationPropertyName,
			out IEnumerable<ODataPath> referenceLinks)
		{
			var navigationProperty = oDataEntity.NavigationProperties.FirstOrDefault(p => p.Name == navigationPropertyName);
			if (navigationProperty != null)
			{
				var collectionValue = navigationProperty.Value as IEnumerable<ODataPath>;
				if (collectionValue != null)
				{
					referenceLinks = collectionValue;
					return true;
				}

				var value = navigationProperty.Value as ODataPath;
				if (value != null)
				{
					referenceLinks = value.Enumerate();
					return true;
				}
			}

			referenceLinks = null;
			return false;
		}
	}
}