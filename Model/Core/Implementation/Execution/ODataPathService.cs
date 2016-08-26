// <copyright file="ODataPathService.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Core.UriParser.Semantic;

	public class ODataPathService : IODataPathService
	{
		public INavigatable GetNavigationTarget(IModelContext modelContext, IEnumerable<ODataPathSegment> oDataPath)
		{
			if (!oDataPath.Any())
			{
				throw new ArgumentException("The path must not be empty.", nameof(oDataPath));
			}

			var translator = new NavigatablePathSegmentTranslator(modelContext);
			var navigatables = oDataPath.Reverse().Select(segment => segment.TranslateWith(translator));

			return navigatables.First(n => n != null);
		}

		public IEntitySet GetNavigationRoot(IModelContext modelContext, IEnumerable<ODataPathSegment> oDataPath)
		{
			if (!oDataPath.Any())
			{
				throw new ArgumentException("The path must not be empty.", nameof(oDataPath));
			}

			var translator = new NavigatablePathSegmentTranslator(modelContext);
			var root = oDataPath.First().TranslateWith(translator);
			return root.As<IEntitySet>();
		}
	}
}