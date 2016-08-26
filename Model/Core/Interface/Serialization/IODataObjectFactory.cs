// <copyright file="IODataObjectFactory.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.Serialization
{
	using Facton.ServiceApi.Core;
	using Microsoft.OData.Core;
	using Microsoft.OData.Edm;

	public interface IODataObjectFactory
	{

		ODataEntityDto CreateODataEntityDto(IEdmEntityType edmType);

		ODataComplexValue CreateODataComplexValue(IEdmComplexType edmType);

		ODataCollectionValue CreateODataCollectionValue(IEdmCollectionType edmType);
	}
}