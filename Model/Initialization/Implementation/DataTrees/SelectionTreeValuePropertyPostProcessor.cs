using System;
using ServiceApi.Model.Core.Builders;
using ServiceApi.Model.Entities.Builders;
using ServiceApi.Model.Initialization.Configuration;
using System.Linq;
using Facton.Infrastructure.Metadata;
using Facton.Infrastructure.Core;

namespace ServiceApi.Model.Initialization.Trees
{
	/// <summary>
	/// A post processor which adds navigation targets to all selection tree value navigation properties so that they navigate to
	/// their correct entitry set.
	/// </summary>
	public class SelectionTreeValuePropertyPostProcessor : ISignatureTypeInitializer, IPostProcessor
	{
		private readonly ISignatureEntitySetInitializer signatureSetInitializer;

		public SelectionTreeValuePropertyPostProcessor(ISignatureEntitySetInitializer signatureSetInitializer)
		{
			this.signatureSetInitializer = signatureSetInitializer;
		}

		public bool CanHandleSignatureType(SignatureTypeConfiguration config)
		{
			// we don't want to initialize any signature types but only perform post processing.
			return false;
		}

		public IEntityTypeBuilder Initialize(SignatureTypeConfiguration config, IBindableModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType)
		{
			throw new NotImplementedException();
		}

		public void PerformPostProcessing(IModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType)
		{
			this.AddDataTreePropertyNavigationTargets(modelBuilder, publicEntityType);
		}

		private void AddDataTreePropertyNavigationTargets(IModelBuilder modelBuilder, IEntityTypeBuilder publicEntityType)
		{
			var entitySetsOfPublicEntityType =
				modelBuilder.EntitySetBuilders.Where(entitySet => entitySet.ContainedEntityType.IsDerivedFrom(publicEntityType)).ToArray();

			foreach (var navigationProperty in publicEntityType.NavigationPropertyBuilders.OfType<IUncontainedNavigationPropertyBuilder>())
			{
				IEntitySetBuilder targetEntitySet;
				if (this.TryGetDataTreeTargetEntitySet(navigationProperty, modelBuilder, out targetEntitySet))
				{
					entitySetsOfPublicEntityType.ForEach(s => s.WithUncontainedNavigationPropertyTarget(navigationProperty, targetEntitySet));
				}
			}
		}

		private bool TryGetDataTreeTargetEntitySet(
			INavigationPropertyBuilder navigationProperty,
			IModelBuilder modelBuilder,
			out IEntitySetBuilder targetSet)
		{
			ISignature treeSignature;
			var costSharpNavigationProperty = navigationProperty as ICostSharpNavigationPropertyBuilder;
			if (costSharpNavigationProperty != null && costSharpNavigationProperty.SourceProperty.TryGetTreeSignature(out treeSignature))
			{
				return modelBuilder.TryGetEntitySetBuilder(this.signatureSetInitializer.GetSetName(treeSignature), out targetSet);
			}

			targetSet = null;
			return false;
		}
	}
}
