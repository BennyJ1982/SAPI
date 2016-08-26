namespace Facton.ServiceApi.Domain.Model.Entities
{
	using System.Collections.Generic;

	using Facton.Infrastructure.Entities;
	using Facton.ServiceApi.Domain.Model.Core;

	public class NavigationPropertyDependency : IDependency
	{
		private readonly INavigationProperty navigationProperty;

		private readonly IEnumerable<IEntity> entities;

		public NavigationPropertyDependency(INavigationProperty navigationProperty, IEnumerable<IEntity> entitiesValue)
		{
			this.navigationProperty = navigationProperty;
			this.entities = entitiesValue;
		}

		public object DependableElement => this.navigationProperty;

		public object Value => this.entities;
	}
}
