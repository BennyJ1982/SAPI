// <copyright file="ODataObjectFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Serialization
{
	using Facton.ServiceApi.Core;
	using Microsoft.OData.Core;
	using Microsoft.OData.Edm;

	public class ODataObjectFactory : IODataObjectFactory
	{
		public ODataEntityDto CreateODataEntityDto(IEdmEntityType edmType)
		{
			return new ODataEntityDto()
			{
				Entry = new ODataEntry() { TypeName = edmType.FullTypeName() }
			};
		}

		public ODataComplexValue CreateODataComplexValue(IEdmComplexType edmType)
		{
			return new ODataComplexValue()
			{
				TypeName = edmType.FullTypeName()
			};
		}

		public ODataCollectionValue CreateODataCollectionValue(IEdmCollectionType edmType)
		{
			return new ODataCollectionValue()
			{
				TypeName = edmType.FullTypeName()
			};
		}
	}
}