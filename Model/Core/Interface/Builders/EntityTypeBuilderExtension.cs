namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	using System.Collections.Generic;

	public static class EntityTypeBuilderExtension
	{
		public static IEnumerable<INavigationPropertyBuilder> GetEffectiveNavigationProperties(this IEntityTypeBuilder entityTypeBuilder)
		{
			var navigationProperties = new List<INavigationPropertyBuilder>();
			do
			{
				navigationProperties.AddRange(entityTypeBuilder.NavigationPropertyBuilders);
				entityTypeBuilder = entityTypeBuilder.ParentTypeBuilder;
			}
			while (entityTypeBuilder != null);

			return navigationProperties;
		}

		/// <summary>
		/// Determines whether this entity type is derived from or is equal to the the specified parent entity type.
		/// </summary>
		/// <param name="entityTypeBuilder">The derived entity type builder.</param>
		/// <param name="parentEntityTypeBuilder">The parent entity type builder.</param>
		public static bool IsDerivedFrom(this IEntityTypeBuilder entityTypeBuilder, IEntityTypeBuilder parentEntityTypeBuilder)
		{
			var parent = entityTypeBuilder.ParentTypeBuilder;
			while (parent != null)
			{
				if (parent.Equals(parentEntityTypeBuilder))
				{
					return true;
				}

				parent = parent.ParentTypeBuilder;
			}

			return false;
		}
	}
}
