namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using System.Collections.Generic;
	using System.Linq;

	using Facton.ServiceApi.Domain.Model.Core.DataTypes;

	public static class ModelBuilderExtension
	{
		public static IModelBuilder WithComplexDataTypes(this IModelBuilder modelBuilder, IEnumerable<IComplexDataType> complexDataTypes)
		{
			return complexDataTypes.Aggregate(modelBuilder, (current, complexDataType) => current.WithComplexDataType(complexDataType));
		}
	}
}
