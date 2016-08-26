// <copyright file="DefaultDataTypeKeyLookupStrategy.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Facton.Infrastructure.Metadata;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes.Clr;

	public class DefaultDataTypeKeyLookupStrategy : IDataTypeKeyLookupStrategy
	{
		public IDataType GetByKey(Func<object, IDataType> baseLookup, object key)
		{
			// look up by string, then by domain type, then by type
			var dataType = GetByString(baseLookup, key) ?? (GetByDomainType(baseLookup, key) ?? GetByType(baseLookup, key));
			return dataType;
		}

		private static IDataType GetByString(Func<object, IDataType> baseLookup, object key)
		{
			var stringKey = key as string;
			if (stringKey != null)
			{
				return baseLookup(stringKey);
			}

			return null;
		}

		private static IDataType GetByDomainType(Func<object, IDataType> baseLookup, object key)
		{
			var domainTypeKey = key as IDomainType;
			if (domainTypeKey != null)
			{
				return baseLookup(domainTypeKey.Name);
			}

			return null;
		}

		private static IDataType GetByType(Func<object, IDataType> baseLookup, object key)
		{
			var typeKey = key as Type ?? key.GetType();

			var dataType = baseLookup(typeKey) ?? ClrDataTypes.GetByClrType(typeKey);
			if (dataType != null)
			{
				return dataType;
			}

			// try with base types and interfaces
			foreach (var baseType in GetAllBaseTypesAndInterfaces(typeKey))
			{
				dataType = baseLookup(baseType) ?? ClrDataTypes.GetByClrType(baseType);
				if (dataType != null)
				{
					return dataType;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns all base types of the specified type, including interfaces.
		/// </summary>
		private static IEnumerable<Type> GetAllBaseTypesAndInterfaces(Type type)
		{
			return GetAllBaseTypes(type).Union(type.GetInterfaces());
		}

		private static IEnumerable<Type> GetAllBaseTypes(Type type)
		{
			for (; type != null; type = type.BaseType) yield return type;
		}
	}
}