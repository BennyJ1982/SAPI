namespace Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes
{
	using Facton.Infrastructure.Core;
	using Facton.ServiceApi.Domain.Model.Core.Queries;

	public class EntityIdQueryAttribute : IQueryAttribute
	{
		public EntityIdQueryAttribute(IId entityId)
		{
			this.EntityId = entityId;
		}

		public IId EntityId { get; }

		public string FqlQueryTextFragment => $"_Id=\"{this.EntityId}\"";
	}
}
