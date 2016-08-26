namespace Facton.Spikes.ServiceApi.ODataMapping.Model.QueryProviders.QueryAttributes
{
	using Facton.Spikes.ServiceApi.ODataMapping.Queries;

	public class SpaceQueryAttribute : IQueryAttribute
	{
		public SpaceQueryAttribute(string space)
		{
			this.Space = space;
		}

		public string Space { get; }

		public string FqlQueryTextFragment => $"SPACE {this.Space}";
	}
}
