// <copyright file="ModelBuilderFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using Facton.ServiceApi.Domain.Model.Core.Builders.Navigation;
	using Facton.ServiceApi.Domain.Model.Core.Execution;

	public class ModelBuilderFactory : IModelBuilderFactory
	{
		private readonly IODataPathService pathTranslator;

		private readonly INavigationPropertyBuilderFactory navigationPropertyBuilderFactory;

		public ModelBuilderFactory(IODataPathService pathTranslator, INavigationPropertyBuilderFactory navigationPropertyBuilderFactory)
		{
			this.pathTranslator = pathTranslator;
			this.navigationPropertyBuilderFactory = navigationPropertyBuilderFactory;
		}

		public IModelBuilder Create()
		{
			return new ModelBuilder(this.pathTranslator, this.navigationPropertyBuilderFactory);
		}
	}
}