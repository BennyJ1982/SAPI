// <copyright file="INavigationPropertyBuilderFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders.Navigation
{
	using Facton.Infrastructure.Metadata;

	public interface INavigationPropertyBuilderFactory
	{
		IContainedNavigationPropertyBuilder CreateContainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			string sourcePropertyName);

		IContainedNavigationPropertyBuilder CreateContainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			IProperty sourceProperty);

		IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			string sourcePropertyName);

		IUncontainedNavigationPropertyBuilder CreateUncontainedNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			IProperty sourceProperty);
	}
}