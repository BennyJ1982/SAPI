// <copyright file="DataTypeRegistry.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using System.Collections.Generic;
	using System.Linq;

	public class DataTypeRegistry : IDataTypeRegistry
	{
		private readonly Dictionary<object, IDataType> registeredItems;

		private readonly IDataTypeKeyLookupStrategy keyLookupStrategy;

		public DataTypeRegistry(IDataTypeKeyLookupStrategy keyLookupStrategy)
		{
			this.registeredItems = new Dictionary<object, IDataType>();
			this.keyLookupStrategy = keyLookupStrategy;
		}

		public void Register(object key, IDataType dataType)
		{
			this.registeredItems.Add(key, dataType);
		}

		public void Register(IEnumerable<object> keys, IDataType dataType)
		{
			foreach (var key in keys)
			{
				this.Register(key, dataType);
			}
		}

		public IEnumerable<IDataType> GetAll()
		{
			return this.registeredItems.Values.Distinct();
		}

		public IDataType GetByKey(object key)
		{
			return this.keyLookupStrategy.GetByKey(this.InternalGetByKey, key);
		}

		private IDataType InternalGetByKey(object key)
		{
			IDataType dataType;
			if (this.registeredItems.TryGetValue(key, out dataType))
			{
				return dataType;
			}

			return null;
		}
	}
}