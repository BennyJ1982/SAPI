namespace Facton.ServiceApi.Domain.Model.Initialization.Common.QueryAttributes
{
	using System;
	using System.Text;

	using Facton.ServiceApi.Domain.Model.Core.Queries;

	public class EntityTypesQueryAttribute : IQueryAttribute
	{
		public EntityTypesQueryAttribute(string[] entityTypes)
		{
			if (entityTypes.Length == 0)
			{
				throw new ArgumentException("At least one entity type has to be specified", nameof(entityTypes));
			}

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
