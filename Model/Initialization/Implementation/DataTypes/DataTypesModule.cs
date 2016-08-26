// <copyright file="DataTypesModule.cs" company="Facton GmbH">
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

namespace Facton.ServiceApi.Domain.Model.Initialization.DataTypes
{
	using System;

	using Facton.Infrastructure.Core;
	using Facton.Infrastructure.Metadata;
	using Facton.Infrastructure.Metadata.ContentLocalization;
	using Facton.Infrastructure.Metadata.DomainValues;
	using Facton.Infrastructure.Metadata.DomainValues.BooleanValues;
	using Facton.Infrastructure.Metadata.Measurement;
	using Facton.Infrastructure.Modularity;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes;
	using Facton.ServiceApi.Domain.Model.Core.DataTypes.Clr;
	using Facton.ServiceApi.Domain.Model.Core.Serialization;

	/// <summary>
	/// Module that initializes the data types.
	/// </summary>
	public class DataTypesModule : IModule
	{
		/// <summary>
		/// This Operation is called, when the module shall initialize itself.
		/// </summary>
		/// <param name="typeRegistry">The type registry for service requests or registrations.</param>
		/// <inheritdoc/>
		public void Initialize(ITypeRegistry typeRegistry)
		{
			var metadataService = typeRegistry.GetObject<IMetadataService>();
			var oDataObjectFactory = typeRegistry.GetObject<IODataObjectFactory>();
			var registry = typeRegistry.GetObject<IDataTypeRegistry>();

			RegisterDataTypes(registry, oDataObjectFactory, metadataService);
		}

		private static void RegisterDataTypes(IDataTypeRegistry registry, IODataObjectFactory oDataObjectFactory, IMetadataService metadataService)
		{
			// FACTON domain types with their own, complex domain values
			registry.Register<IUnitValueDomainType>(new UnitValueDataType(oDataObjectFactory));
			registry.Register<ILocalizedLongTextDomainType>(new LocalizedTextDataType(oDataObjectFactory));

			// FACTON domain types whose domain values can be mapped to CLR types
			registry.Register<AbstractDateTimeDomainType>(new ClrTypeReferencingDataType<DateValue, DateTimeOffset>(
				v => new DateTimeOffset(v.Date), v => new DateValue(v.DateTime.ToUniversalTime())));

			registry.Register("Boolean", new ClrTypeReferencingDataType<BooleanValue, bool>(v => v.Value, v => v.ToBooleanValue()));
			registry.Register("Number", new ClrTypeReferencingDataType<NumberValue, decimal>(v => v.InternalNumber, v => new NumberValue(v)));
			registry.Register(
				new object[] { "Integer", "AutoIncrement" },
				new ClrTypeReferencingDataType<IntegerValue, long>(v => v.Value, v => new IntegerValue(v)));
			registry.Register(
				new object[] { "Id", "ContextBoundId" },
				new ClrTypeReferencingDataType<IId, string>(IdConverter.ConvertToString, IdConverter.ConvertToId));
			registry.Register<IMetadataItemDomainType>(new MetadataItemDataType(metadataService));

			// FACTON domain types whose values _are_ CLR types 
			registry.Register("TimeSpan", ClrDataTypes.Duration);
			registry.Register("SimpleBoolean", ClrDataTypes.Boolean);
			registry.Register("SimpleInteger", ClrDataTypes.Int32);
			registry.Register("SimpleDecimal", ClrDataTypes.Decimal);
			registry.Register("SimpleLong", ClrDataTypes.Int64);
			registry.Register("SimpleString", ClrDataTypes.String);
			registry.Register("SimpleLongString", ClrDataTypes.String);

			// FACTON domain types whose values can be serialized/deserialized to strings, using the DomainType itself
			registry.Register<ILongTextDomainType, ICurrencyDomainType>(new SerializableDomainTypeDataType());

			// other, non-domain types
			registry.Register<IId>(new ClrTypeReferencingDataType<IId, string>(IdConverter.ConvertToString, IdConverter.ConvertToId));
		}
	}
}