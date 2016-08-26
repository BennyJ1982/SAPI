// <copyright file="UncontainedCostSharpNavigationPropertyBuilder.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders.Navigation
{
	using Facton.Infrastructure.Metadata;

	public class UncontainedCostSharpNavigationPropertyBuilder : UncontainedNavigationPropertyBuilder, ICostSharpNavigationPropertyBuilder
	{
		public UncontainedCostSharpNavigationPropertyBuilder(
			IEntityTypeBuilder targetEntityTypeBuilder,
			IProperty sourceProperty,
			string sourcePropertyName)
			: base(targetEntityTypeBuilder, sourcePropertyName)
		{
			this.SourceProperty = sourceProperty;
		}

		public IProperty SourceProperty { get; }
	}
}