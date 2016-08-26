namespace Facton.ServiceApi.Domain.Model.Entities.Builders
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Facton.ServiceApi.Domain.Model.Core;
	using Facton.ServiceApi.Domain.Model.Core.Builders;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.Execution.Handlers;
	using Facton.ServiceApi.Domain.Model.Entities.Bindings;

	public class BindableModelBuilder : IBindableModelBuilder
	{
		private readonly IModelBuilder underlyingModelBuilder;

		private readonly IList<KeyValuePair<INavigatableElementBuilder, DependencyDeclaration>> dependencies =
			new List<KeyValuePair<INavigatableElementBuilder, DependencyDeclaration>>();

		private readonly IDictionary<INavigatableElementBuilder, IBinding> bindings = new Dictionary<INavigatableElementBuilder, IBinding>();

		public BindableModelBuilder(IModelBuilder underlyingModelBuilder)
		{
			this.underlyingModelBuilder = underlyingModelBuilder;
		}

		public IEnumerable<IEntityTypeBuilder> EntityTypeBuilders => this.underlyingModelBuilder.EntityTypeBuilders;

		public IEnumerable<IEntitySetBuilder> EntitySetBuilders => this.underlyingModelBuilder.EntitySetBuilders;

		public IModelBuilder WithNamespace(string nameSpace) => this.underlyingModelBuilder.WithNamespace(nameSpace);

		public IModelBuilder WithContainerName(string containerName) => this.underlyingModelBuilder.WithContainerName(containerName);

		public IModelBuilder WithComplexDataType(IComplexDataType complexDataType)
			=> this.underlyingModelBuilder.WithComplexDataType(complexDataType);

		public IModelBuilder WithOperationHandler(INavigatableElementBuilder navigatableElementBuilder, IOperationHandler operationHandler)
			=> this.underlyingModelBuilder.WithOperationHandler(navigatableElementBuilder, operationHandler);

		public IModelBuilder WithOperationHandler(IOperationHandler operationHandler)
			=> this.underlyingModelBuilder.WithOperationHandler(operationHandler);

		public IEntityTypeBuilder CreateEntityTypeBuilder(string name) => this.underlyingModelBuilder.CreateEntityTypeBuilder(name);

		public IEntitySetBuilder CreateEntitySetBuilder(string name, IEntityTypeBuilder containedEntityTypeBuilder)
			=> this.underlyingModelBuilder.CreateEntitySetBuilder(name, containedEntityTypeBuilder);

		public bool TryGetEntityTypeBuilder(string name, out IEntityTypeBuilder entityTypeBuilder)
			=> this.underlyingModelBuilder.TryGetEntityTypeBuilder(name, out entityTypeBuilder);


		public bool TryGetEntitySetBuilder(string name, out IEntitySetBuilder entitySetBuilder)
			=> this.underlyingModelBuilder.TryGetEntitySetBuilder(name, out entitySetBuilder);

		public IModelContext Build()
		{
			this.ValidateEntitySetBindings();
			this.ValidateNavigaitonPropertyBindings();

			var underlyingContext = this.underlyingModelBuilder.Build();

			return new BindableModelContext(
				underlyingContext,
				this.GetDependencies(),
				this.bindings.ToDictionary(b => b.Key.BuiltElement, b => b.Value));
		}

		public IBindableModelBuilder WithDependency(INavigatableElementBuilder navigatableElementBuilder, IStructuralProperty dependency)
		{
			var declaration = new DependencyDeclaration(dependency);
			this.dependencies.Add(new KeyValuePair<INavigatableElementBuilder, DependencyDeclaration>(navigatableElementBuilder, declaration));
			return this;
		}

		public IBindableModelBuilder WithDependency(INavigatableElementBuilder navigatableElementBuilder, INavigationPropertyBuilder dependency)
		{
			var declaration = new DependencyDeclaration(dependency);
			this.dependencies.Add(new KeyValuePair<INavigatableElementBuilder, DependencyDeclaration>(navigatableElementBuilder, declaration));
			return this;
		}

		public IBindableModelBuilder WithOptionalDependency(INavigatableElementBuilder navigatableElementBuilder, IStructuralProperty dependency)
		{
			var declaration = new DependencyDeclaration(dependency, true);
			this.dependencies.Add(new KeyValuePair<INavigatableElementBuilder, DependencyDeclaration>(navigatableElementBuilder, declaration));
			return this;
		}

		public IBindableModelBuilder WithOptionalDependency(
			INavigatableElementBuilder navigatableElementBuilder,
			INavigationPropertyBuilder dependency)
		{
			var declaration = new DependencyDeclaration(dependency, true);
			this.dependencies.Add(new KeyValuePair<INavigatableElementBuilder, DependencyDeclaration>(navigatableElementBuilder, declaration));
			return this;
		}

		public IBindableModelBuilder WithBinding(INavigatableElementBuilder navigatableElementBuilder, IBinding binding)
		{
			this.bindings[navigatableElementBuilder] = binding;
			return this;
		}

		public bool TryGetBinding<T>(INavigatableElementBuilder navigatableElementBuilder, out T binding) where T : class, IBinding
		{
			IBinding bindingValue;
			if (this.bindings.TryGetValue(navigatableElementBuilder, out bindingValue))
			{
				binding = bindingValue as T;
				return binding != null;
			}

			binding = null;
			return false;
		}

		private IDictionary<INavigatable, IEnumerable<DependencyDeclaration>> GetDependencies()
		{
			return this.dependencies.GroupBy(d => d.Key.BuiltElement, d => TranslateDependency(d.Value))
				.ToDictionary(d => d.Key, d => d.ToArray().AsEnumerable());
		}

		private static DependencyDeclaration TranslateDependency(DependencyDeclaration declaration)
		{
			var navigationPropertyBuilder = declaration.DependableElement as INavigationPropertyBuilder;
			if (navigationPropertyBuilder != null)
			{
				return new DependencyDeclaration(navigationPropertyBuilder.BuiltNavigationProperty, declaration.IsOptional);
			}

			return declaration;
		}

		private void ValidateEntitySetBindings()
		{
			foreach (var entitySet in this.underlyingModelBuilder.EntitySetBuilders)
			{
				ISingletonBinding singletonBinding;
				IEntitySetBinding entitySetBinding;

				if (entitySet.IsSingleton && !this.TryGetBinding(entitySet, out singletonBinding))
				{
					throw new InvalidOperationException("Binding must be a singleton binding.");
				}

				if (!entitySet.IsSingleton && !this.TryGetBinding(entitySet, out entitySetBinding))
				{
					throw new InvalidOperationException("Binding must be an entity set binding.");
				}
			}
		}

		private void ValidateNavigaitonPropertyBindings()
		{
			foreach (var property in this.underlyingModelBuilder.EntityTypeBuilders.SelectMany(e => e.NavigationPropertyBuilders))
			{
				if (property.IsCollection)
				{
					IContainedCollectionNavigationPropertyBinding containedCollectionBinding;
					IUncontainedCollectionNavigationPropertyBinding uncontainedCollectionBinding;

					if (property is IContainedNavigationPropertyBuilder && !this.TryGetBinding(property, out containedCollectionBinding))
					{
						throw new InvalidOperationException("Binding must be a contained collection navigation property binding.");
					}

					if (property is IUncontainedNavigationPropertyBuilder && !this.TryGetBinding(property, out uncontainedCollectionBinding))
					{
						throw new InvalidOperationException("Binding must be an uncontained collection navigation property binding.");
					}
				}
				else
				{
					IContainedNavigationPropertyBinding containedBinding;
					IUncontainedNavigationPropertyBinding uncontainedBinding;

					if (property is IContainedNavigationPropertyBuilder && !this.TryGetBinding(property, out containedBinding))
					{
						throw new InvalidOperationException("Binding must be a contained navigation property binding.");
					}

					if (property is IUncontainedNavigationPropertyBuilder && !this.TryGetBinding(property, out uncontainedBinding))
					{
						throw new InvalidOperationException("Binding must be an uncontained navigation property binding.");
					}
				}
			}
		}
	}
}
