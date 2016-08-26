// <copyright file="NavigationPropertyBuilderFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders.Navigation
{
	using Facton.Infrastructure.Metadata;

	public class NavigationPropertyBuilderFactory : INavigationPropertyBuilderFactory
	{
		private readonly IModelItemNamingService modelItemNamingService;

		public NavigationPropertyBuilderFactory(IModelItemNamingService modelItemNamingService)
		{
			this.modelItemNamingService = modelItemNamingService;
		}

		public IContainedNavigationPropertyBuilder CreateContainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			string sourcePropertyName)
		{
			return new ContainedNavigationPropertyBuilder(targetEntityTypeBuilder, sourcePropertyName);
		}

		public IContainedNavigationPropertyBuilder CreateContainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			IProperty sourceProperty)
		{
			return new ContainedCostSharpNavigationPropertyBuilder(
				targetEntityTypeBuilder,
				sourceProperty,
				this.modelItemNamingService.GetSafeEdmPropertyName(sourceProperty));
		}

		public IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			string sourcePropertyName)
		{
			return new UncontainedNavigationPropertyBuilder(targetEntityTypeBuilder, sourcePropertyName);
		}

		public IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			IProperty sourceProperty)
		{
			return new UncontainedCostSharpNavigationPropertyBuilder(
				targetEntityTypeBuilder,
				sourceProperty,
				this.modelItemNamingService.GetSafeEdmPropertyName(sourceProperty));
		}
	}
}