namespace Facton.ServiceApi.Domain.Model.Core.DataTypes
{
	public static class DataTypeRegistryExtension
	{
		public static void Register<T>(this IDataTypeRegistry registry, IDataType dataType)
		{
			registry.Register(typeof(T), dataType);
		}

		public static void Register<T1, T2>(this IDataTypeRegistry registry, IDataType dataType)
		{
			registry.Register(new object[] { typeof(T1), typeof(T2) }, dataType);
		}

		public static void Register<T1, T2, T3>(this IDataTypeRegistry registry, IDataType dataType)
		{
			registry.Register(new object[] { typeof(T1), typeof(T2), typeof(T3) }, dataType);
		}

		public static void Register<T1, T2, T3, T4>(this IDataTypeRegistry registry, IDataType dataType)
		{
			registry.Register(new object[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, dataType);
		}
	}
}
