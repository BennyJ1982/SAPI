namespace Facton.ServiceApi.Domain.Model.Initialization
{
	using System.Collections.Generic;

	public interface IEntityTypeInitializerRegistry
	{
		void RegisterEntityTypeInitializer(IEntityTypeInitializer entityTypeInitializer);

		IEnumerable<IEntityTypeInitializer> GetAll();
	}
}
