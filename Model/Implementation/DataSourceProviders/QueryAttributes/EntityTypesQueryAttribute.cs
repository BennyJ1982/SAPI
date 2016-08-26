namespace Facton.Spikes.ServiceApi.ODataMapping.Model.QueryProviders.QueryAttributes
{
	using System.Text;
	using Facton.Spikes.ServiceApi.ODataMapping.Queries;

	public class EntityTypesQueryAttribute : IQueryAttribute
	{
		public EntityTypesQueryAttribute(string[] entityTypes)
		{
			this.EntityTypes = entityTypes;
		}

		public string[] EntityTypes { get; }

		public string FqlQueryTextFragment
		{
			get
			{
				var stringBuilder = new StringBuilder("Types ");

				for (int a = 0; a < this.EntityTypes.Length; a++)
				{
					stringBuilder.Append(this.EntityTypes[a]);

					if (a < this.EntityTypes.Length - 1)
					{
						stringBuilder.Append(", ");
					}
				}

				return stringBuilder.ToString();
			}
		}
	}
}
