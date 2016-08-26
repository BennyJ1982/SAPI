// <copyright file="IODataRepository.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Core
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.OData.Core.UriParser.Semantic;
	using Microsoft.OData.Edm;

	public interface IODataRepository
	{
		IEdmModel GetModel();

		bool CanPost(ODataPath path);

		bool CanPatch(ODataPath path);

		bool CanGet(ODataPath path);

		Task<ODataEntityDto> Post(ODataPath path, QueryOptions queryOptions, ODataEntityDto incomingObject);

		Task<ODataEntityDto> Patch(ODataPath path, QueryOptions queryOptions, ODataEntityDto incomingObject);

		Task<IEnumerable<ODataEntityDto>> Get(ODataPath path, QueryOptions queryOptions);
	}
}