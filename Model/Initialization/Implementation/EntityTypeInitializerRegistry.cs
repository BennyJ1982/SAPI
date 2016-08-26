namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Collections.Generic;

	public class EntityTypeInitializerRegistry : IEntityTypeInitializerRegistry
	{
		private readonly IList<IEntityTypeInitializer> initializers = new List<IEntityTypeInitializer>();

		public void RegisterEntityTypeInitializer(IEntityTypeInitializer entityTypeInitializer)
		{
			this.initializers.Add(entityTypeInitializer);
		}

		public IEnumerable<IEntityTypeInitializer> GetAll()
		{
			return this.initializers;
		}
	}
}
