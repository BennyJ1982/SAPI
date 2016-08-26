namespace Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes
{
	using Facton.ServiceApi.Domain.Model.Core.Queries;

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
