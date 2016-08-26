namespace Facton.Spikes.ServiceApi.ODataMapping.WebFrameworks.WebApi.UriParsing
{
    using System;
    using System.Linq;
    using System.Web.OData.Routing;
    using Facton.Infrastructure.Core;
    using Facton.Spikes.ServiceApi.ODataMapping.Model.Core;
    using Facton.Spikes.ServiceApi.ODataMapping.Model.EntitySets;
    using Facton.Spikes.ServiceApi.ODataMapping.Model.EntityTypes;
    using Facton.Spikes.ServiceApi.ODataMapping.Model.Navigation;

    /// <summary>
    /// PathSegmentBuildInfo factory which can handle WebApi OData path segments
    /// </summary>
    public class PathSegmentBuildInfoFactory : IPathSegmentBuildInfoFactory<ODataPathSegment>
    {
        private readonly IEntitySetRegistry entitySetRegistry;

        public PathSegmentBuildInfoFactory(IEntitySetRegistry entitySetRegistry)
        {
            this.entitySetRegistry = entitySetRegistry;
        }

        public PathSegmentBuildInfo CreatePathSegmentBuildInfo(ODataPathSegment segment, Func<PathSegment> getPreviouslyBuiltSegment)
        {
            var navigationSegment = segment as NavigationPathSegment;
            if (navigationSegment != null)
            {
                return new PathSegmentBuildInfo(
                    PathSegmentBuildInfo.PathSegmentKind.Navigation,
                    navigationSegment.NavigationPropertyName,
                    new Lazy<IEntityType>(
                        () =>
                        GetNavigationProperty(getPreviouslyBuiltSegment().EntityType, navigationSegment.NavigationPropertyName).TargetType),
                    new Lazy<EntityOperations>(
                        () =>
                        GetNavigationProperty(getPreviouslyBuiltSegment().EntityType, navigationSegment.NavigationPropertyName)
                            .SupportedSourceEntityOperations));
            }

            var entitySetSegment = segment as EntitySetPathSegment;
            if (entitySetSegment != null)
            {
                return new PathSegmentBuildInfo(
                    PathSegmentBuildInfo.PathSegmentKind.EntitySet,
                    entitySetSegment.EntitySetName,
                    new Lazy<IEntityType>(() => this.entitySetRegistry.GetByKey(entitySetSegment.EntitySetName).EntityType),
                    new Lazy<EntityOperations>(
                        () => this.entitySetRegistry.GetByKey(entitySetSegment.EntitySetName).SupportedEntityOperations));
            }

            var singletonSegment = segment as SingletonPathSegment;
            if (singletonSegment != null)
            {
                return new PathSegmentBuildInfo(
                    PathSegmentBuildInfo.PathSegmentKind.Singleton,
                    singletonSegment.SingletonName,
                    new Lazy<IEntityType>(() => this.entitySetRegistry.GetByKey(singletonSegment.SingletonName).EntityType),
                    new Lazy<EntityOperations>(
                        () => this.entitySetRegistry.GetByKey(singletonSegment.SingletonName).SupportedEntityOperations));
            }

            var keySegment = segment as KeyValuePathSegment;
            return
                new PathSegmentBuildInfo(
                    keySegment != null ? PathSegmentBuildInfo.PathSegmentKind.Key : PathSegmentBuildInfo.PathSegmentKind.Unsupported,
                    null,
                    new Lazy<IEntityType>(() => null),
                    new Lazy<EntityOperations>(() => EntityOperations.None));
        }

        private static NavigationProperty GetNavigationProperty(IEntityType sourceEntityType, string navigationPropertyName)
        {
            return
                sourceEntityType.As<IEntityTypeWithNavigation>()
                    .GetSourceNavigationProperties(true)
                    .First(p => p.SourcePropertyName == navigationPropertyName);
        }
    }
}
