// <copyright file="ODataComplexValueExtension.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Serialization
{
	using System.Linq;

	using Microsoft.OData.Core;

	public static class ODataComplexValueExtension
	{
		public static bool TryGetPropertyValue<T>(this ODataComplexValue complexValue, string propertyName, out T value)
		{
			var numberProperty = complexValue.Properties.FirstOrDefault(p => p.Name == propertyName);
			if (numberProperty != null && numberProperty.Value is T)
			{
				value = (T)numberProperty.Value;
				return true;
			}

			value = default(T);
			return false;
		}
	}
}