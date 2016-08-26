// <copyright file="EntityType.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.OData.Edm;

	/// <summary>
	/// Represents an ODATA entity type.
	/// </summary>
	public class EntityType : IEntityType
	{
		private IReadOnlyDictionary<string, INavigationProperty> navigationProperties;

		private readonly IReadOnlyDictionary<string, IStructuralProperty> keyProperties;

		private readonly IReadOnlyDictionary<string, IStructuralProperty> properties;

		public EntityType(
			string name,
			IEdmEntityType resultingEdmEntityType,
			IReadOnlyDictionary<string, IStructuralProperty> keyProperties,
			IReadOnlyDictionary<string, IStructuralProperty> properties)
		{
			this.Name = name;
			this.ResultingEdmEntityType = resultingEdmEntityType;
			this.keyProperties = keyProperties;
			this.properties = properties;
		}

		public string Name { get; }

		public IEdmEntityType ResultingEdmEntityType { get; }

		public IEnumerable<IStructuralProperty> StructuralProperties => this.properties.Values;

		public IEnumerable<IStructuralProperty> KeyProperties => this.keyProperties.Values;

		public bool TryGetStructuralProperty(string propertyName, out IStructuralProperty property)
		{
			return this.properties.TryGetValue(propertyName, out property);
		}

		public bool TryGetKeyProperty(string propertyName, out IStructuralProperty keyProperty)
		{
			return this.keyProperties.TryGetValue(propertyName, out keyProperty);
		}

		public bool TryGetNavigationProperty(string sourcePropertyName, out INavigationProperty navigationProperty)
		{
			if (this.navigationProperties == null)
			{
				throw new InvalidOperationException("Navigation properties haven't been set when building this entity type.");
			}

			return this.navigationProperties.TryGetValue(sourcePropertyName, out navigationProperty);
		}

		internal void SetNavigationProperties(IEnumerable<INavigationProperty> navigationProperties)
		{
			if (this.navigationProperties != null)
			{
				throw new InvalidOperationException("Navigation properties have been already set.");
			}

			this.navigationProperties = navigationProperties.ToDictionary(n => n.Name, n => n);
		}
	}
}