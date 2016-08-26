// <copyright file="ODataEntityDto.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Core
{
	using System.Collections.Generic;
	using Microsoft.OData.Core;

	/// <summary>
	/// Represents an entity dto that can have navigation properties.
	/// </summary>
	public sealed class ODataEntityDto
	{
		/// <summary>
		/// Gets or sets the ODataEntry for this entity dto.
		/// </summary>
		public ODataEntry Entry { get; set; }

		/// <summary>
		/// Gets or sets the navigation properties of this entity dto.
		/// </summary>
		public IEnumerable<ODataNavigationProperty> NavigationProperties { get; set; }
	}
}