// <copyright file="ODataNavigationProperty.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Core
{
	/// <summary>
	/// Represents a navigation property.
	/// </summary>
	public sealed class ODataNavigationProperty
	{
		/// <summary>
		/// Gets or sets the name of the navigation property.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the value of the navigation property.
		/// </summary>
		/// <remarks>
		/// A value could be either a <see cref="ODataEntityDto"/> or a <see cref="Microsoft.OData.Core.UriParser.Semantic.ODataPath"/>
		/// (for non collection navigation properties). If the navigation property is a collection the value would be an
		/// <see cref="System.Collections.IEnumerable"/> of these two possible value types.
		/// </remarks>
		public object Value { get; set; }
	}
}