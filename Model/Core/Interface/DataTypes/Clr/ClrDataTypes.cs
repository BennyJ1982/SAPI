// <copyright file="ClrDataTypes.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Core.DataTypes.Clr
{
	using System;
	using System.Collections.Generic;

	using Microsoft.OData.Edm;

	/// <summary>
	/// Pre-defined data types for all basic CLR types that can be mapped to ODATA primitve types out-of-the-box.
	/// </summary>
	public static class ClrDataTypes
	{
		private static readonly Dictionary<Type, IDataType> RegisteredItems;

		static ClrDataTypes()
		{
			RegisteredItems = new Dictionary<Type, IDataType>();

			Register(String = new ClrDataType<string>(EdmPrimitiveTypeKind.String));
			Register(Boolean = new ClrDataType<bool>(EdmPrimitiveTypeKind.Boolean));
			Register(Byte = new ClrDataType<byte>(EdmPrimitiveTypeKind.Byte));
			Register(Int16 = new ClrDataType<short>(EdmPrimitiveTypeKind.Int16));
			Register(Int32 = new ClrDataType<int>(EdmPrimitiveTypeKind.Int32));
			Register(Int64 = new ClrDataType<long>(EdmPrimitiveTypeKind.Int64));
			Register(Single = new ClrDataType<float>(EdmPrimitiveTypeKind.Single));
			Register(Double = new ClrDataType<double>(EdmPrimitiveTypeKind.Double));
			Register(Decimal = new ClrDataType<decimal>(EdmPrimitiveTypeKind.Decimal));
			Register(DateTimeOffset = new ClrDataType<DateTimeOffset>(EdmPrimitiveTypeKind.DateTimeOffset));
			Register(Duration = new ClrDataType<TimeSpan>(EdmPrimitiveTypeKind.Duration));
			Register(Guid = new ClrDataType<Guid>(EdmPrimitiveTypeKind.Guid));
		}

		public static IClrDataType<string> String { get; }
		public static IClrDataType<bool> Boolean { get; }
		public static IClrDataType<byte> Byte { get; }
		public static IClrDataType<short> Int16 { get; }
		public static IClrDataType<int> Int32 { get; }
		public static IClrDataType<long> Int64 { get; }
		public static IClrDataType<float> Single { get; }
		public static IClrDataType<double> Double { get; }
		public static IClrDataType<decimal> Decimal { get; }
		public static IClrDataType<DateTimeOffset> DateTimeOffset { get; }
		public static IClrDataType<TimeSpan> Duration { get; }
		public static IClrDataType<Guid> Guid { get; }

		public static IClrDataType<TClrType> GetByClrType<TClrType>()
		{
			return (IClrDataType<TClrType>)GetByClrType(typeof(TClrType));
		}

		public static IDataType GetByClrType(Type clrType)
		{
			IDataType dataType;
			if (RegisteredItems.TryGetValue(clrType, out dataType))
			{
				return dataType;
			}

			return null;
		}

		private static void Register<TClrType>(IClrDataType<TClrType> dataType)
		{
			RegisteredItems.Add(typeof(TClrType), dataType);
		}
	}
}