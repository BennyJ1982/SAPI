// <copyright file="ContainedNavigationPropertyBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders.Navigation
{
	using Microsoft.OData.Edm.Library;

	public class ContainedNavigationPropertyBuilder : NavigationPropertyBuilderBase<IContainedNavigationPropertyBuilder>,
													IContainedNavigationPropertyBuilder
	{
		public ContainedNavigationPropertyBuilder(IEntityTypeBuilder targetEntityTypeBuilder, string sourcePropertyName)
			: base(targetEntityTypeBuilder, sourcePropertyName)
		{
		}

		protected override EdmNavigationPropertyInfo CreateNavigationPropertyInfo()
		{
			var info = base.CreateNavigationPropertyInfo();
			info.ContainsTarget = true;
			return info;
		}
	}
}