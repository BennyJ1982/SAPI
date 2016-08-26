namespace Facton.ServiceApi.Domain.Model.Core.Builders
{
	public static class EntitySetBuilderExtension
	{
		public static IEntitySetBuilder WithUncontainedNavigationPropertySelfTarget(
			this IEntitySetBuilder entitySetBuilder,
			IUncontainedNavigationPropertyBuilder navigationProperty)
		{
			return entitySetBuilder.WithUncontainedNavigationPropertyTarget(navigationProperty, entitySetBuilder);
		}
	}
}
