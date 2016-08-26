// <copyright file="BindableModelContext.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System.Collections.Generic;
	using System.Linq;
	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Edm;

	public class BindableModelContext : IBindableModelContext
	{
		private readonly IModelContext underlyingModelContext;

		private readonly IDictionary<INavigatable, IEnumerable<DependencyDeclaration>> dependencies;

		private readonly IDictionary<INavigatable, IBinding> bindings;

		public BindableModelContext(
			IModelContext underlyingModelContext,
			IDictionary<INavigatable, IEnumerable<DependencyDeclaration>> dependencies,
			IDictionary<INavigatable, IBinding> bindings)
		{
			this.underlyingModelContext = underlyingModelContext;
			this.dependencies = dependencies;
			this.bindings = bindings;
		}

		public IEdmModel EdmModel => this.underlyingModelContext.EdmModel;

		public bool TryGetEntityType(string name, out IEntityType entityType)
			=> this.underlyingModelContext.TryGetEntityType(name, out entityType);

		public bool TryGetEntitySet(string name, out IEntitySet entitySet) => this.underlyingModelContext.TryGetEntitySet(name, out entitySet);

		public bool TryGetHandler(IOperationContext operationContext, out IOperationHandler relevantHandler)
			=> this.underlyingModelContext.TryGetHandler(operationContext, out relevantHandler);

		public INavigatable GetNavigationTarget(ODataPath path) => this.underlyingModelContext.GetNavigationTarget(path);

		public bool TryGetBinding<T>(INavigatable navigatable, out T binding) where T : class, IBinding
		{
			IBinding bindingValue;
			if (this.bindings.TryGetValue(navigatable, out bindingValue))
			{
				binding = bindingValue as T;
				return binding != null;
			}

			binding = null;
			return false;
		}

		public IEnumerable<DependencyDeclaration> GetDependencies(INavigatable navigatable)
		{
			IEnumerable<DependencyDeclaration> resolvedDependencies;
			return this.dependencies.TryGetValue(navigatable, out resolvedDependencies)
						? resolvedDependencies
						: Enumerable.Empty<DependencyDeclaration>();
		}
	}
}