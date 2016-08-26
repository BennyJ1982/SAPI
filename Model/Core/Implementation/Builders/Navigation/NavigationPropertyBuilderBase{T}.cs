// <copyright file="NavigationPropertyBuilderBase{T}.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Builders.Navigation
{
	using System;
	using Facton.ServiceApi.Domain.Model.Core.Execution;
	using Microsoft.OData.Edm;
	using Microsoft.OData.Edm.Library;

	public abstract class NavigationPropertyBuilderBase<TBuilder> : INavigationPropertyBuilder
		where TBuilder : class, INavigationPropertyBuilder
	{
		private EdmMultiplicity sourceMultiplicity;

		private EdmMultiplicity targetMultiplicity;

		private Operation supportedOperations;

		protected NavigationPropertyBuilderBase(IEntityTypeBuilder targetEntityTypeBuilder, string sourcePropertyName)
		{
			this.TargetEntityTypeBuilder = targetEntityTypeBuilder;
			this.SourcePropertyName = sourcePropertyName;
		}

		public bool AlreadyBuilt => this.BuiltNavigationProperty != null;

		public INavigatable BuiltElement => this.BuiltNavigationProperty;

		public INavigationProperty BuiltNavigationProperty { get; private set; }

		public string SourcePropertyName { get; }

		public IEntityTypeBuilder TargetEntityTypeBuilder { get; }

		public bool IsCollection => this.targetMultiplicity == EdmMultiplicity.Many;

		public TBuilder WithSupportedOperations(Operation operation)
		{
			this.ThrowIfAlreadyBuilt();
			this.supportedOperations = operation;
			return (TBuilder)(INavigationPropertyBuilder)this;
		}

		public TBuilder WithMultiplicity(EdmMultiplicity sourceMultiplicity, EdmMultiplicity targetMultiplicity)
		{
			this.ThrowIfAlreadyBuilt();
			this.sourceMultiplicity = sourceMultiplicity;
			this.targetMultiplicity = targetMultiplicity;
			return (TBuilder)(INavigationPropertyBuilder)this;
		}

		public INavigationProperty Build(EdmEntityType sourceEdmEntityType)
		{
			this.Validate();

			var edmNavigationProperty = this.AddToSourceEdmType(sourceEdmEntityType);
			this.BuiltNavigationProperty = this.CreateNavigationProperty(edmNavigationProperty);

			return this.BuiltNavigationProperty;
		}

		protected virtual EdmNavigationPropertyInfo CreateNavigationPropertyInfo()
		{
			return new EdmNavigationPropertyInfo
			{
				Name = this.SourcePropertyName,
				TargetMultiplicity = this.targetMultiplicity,
				Target = this.TargetEntityTypeBuilder.BuiltEntityType.ResultingEdmEntityType,
			};
		}

		private void Validate()
		{
			this.ThrowIfAlreadyBuilt();
			if (this.sourceMultiplicity == 0)
			{
				throw new InvalidOperationException("Source multiplicity must be specified");
			}

			if (this.targetMultiplicity == 0)
			{
				throw new InvalidOperationException("Target multiplicity must be specified");
			}

			if (this.supportedOperations == 0)
			{
				throw new InvalidOperationException("A navigation property has to support at least one operation.");
			}
		}

		private NavigationProperty CreateNavigationProperty(IEdmNavigationProperty edmNavigationProperty)
		{
			return new NavigationProperty(edmNavigationProperty, this.TargetEntityTypeBuilder.BuiltEntityType, this.supportedOperations);

		}

		private IEdmNavigationProperty AddToSourceEdmType(EdmEntityType sourceEdmEntityType)
		{
			return sourceEdmEntityType.AddUnidirectionalNavigation(this.CreateNavigationPropertyInfo());
		}

		private void ThrowIfAlreadyBuilt()
		{
			if (this.AlreadyBuilt)
			{
				throw new InvalidOperationException("This navigation property has already been built.");
			}
		}
	}
}