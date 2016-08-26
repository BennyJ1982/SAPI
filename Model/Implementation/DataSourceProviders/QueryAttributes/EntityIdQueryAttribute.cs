namespace Facton.Spikes.ServiceApi.ODataMapping.Model.QueryProviders.QueryAttributes
{
	using Facton.Infrastructure.Core;
	using Facton.Spikes.ServiceApi.ODataMapping.Queries;

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
