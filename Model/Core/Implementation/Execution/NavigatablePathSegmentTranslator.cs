// <copyright file="NavigatablePathSegmentTranslator.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using System.Collections.Generic;
	using Facton.Infrastructure.Core;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Core.UriParser.Visitors;
	using Microsoft.OData.Edm;

	/// <summary>
	/// A visitor that translates each supported OData path segment into an INavigatable
	/// </summary>
	public class NavigatablePathSegmentTranslator : PathSegmentTranslator<INavigatable>
	{
		private readonly IModelContext modelContext;

		public NavigatablePathSegmentTranslator(IModelContext modelContext)
		{
			this.modelContext = modelContext;
		}

		public override INavigatable Translate(EntitySetSegment segment)
		{
			return this.modelContext.GetEntitySetOrThrow(segment.EntitySet.Name);
		}

		public override INavigatable Translate(SingletonSegment segment)
		{
			return this.modelContext.GetEntitySetOrThrow(segment.Singleton.Name);
		}

		public override INavigatable Translate(NavigationPropertySegment segment)
		{
			var sourceType = this.modelContext.GetEntityTypeOrThrow(segment.NavigationProperty.DeclaringType.As<IEdmEntityType>().Name);
			INavigationProperty property;
			if (sourceType.TryGetNavigationProperty(segment.NavigationProperty.Name, out property))
			{
				return property;
			}

			throw new KeyNotFoundException("Navigation property " + segment.NavigationProperty.Name + " not found on type " + sourceType.Name);
		}

		public override INavigatable Translate(KeySegment segment)
		{
			return null;
		}
	}
}