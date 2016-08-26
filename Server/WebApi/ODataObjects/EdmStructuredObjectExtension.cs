namespace ServiceApi.Server.WebApi.ODataObjects
{
	using System.Web.OData;

	public static class EdmStructuredObjectExtension
	{
		/// <summary>
		/// Gets the property value and wraps it in a DTO wrapper if necessary.
		/// </summary>
		public static bool TryGetPropertyValue<T>(this IEdmStructuredObject targetObject, string propertyName, out T value)
		{
			object internalValue;
			if (targetObject.TryGetPropertyValue(propertyName, out internalValue))
			{
				// wrap by dto wrapper, if applicable
				object wrappedValue;
				if (PropertyValueWrappingHelper.TryWrapEdmObject(internalValue, out wrappedValue))
				{
					internalValue = wrappedValue;
				}

				// now cast to desired type
				if (internalValue is T)
				{
					value = (T)internalValue;
					return true;
				}
			}

			value = default(T);
			return false;
		}

		/// <summary>
		/// Sets the property value and unwraps it from a DTO wrapper if necessary.
		/// </summary>
		public static bool TrySetPropertyValue<T>(this EdmStructuredObject targetObject, string propertyName, T value)
		{
			if (value != null)
			{
				object unwrappedValue;
				if (PropertyValueWrappingHelper.TryUnwrapEdmObject(value, out unwrappedValue))
				{
					return targetObject.TrySetPropertyValue(propertyName, unwrappedValue);
				}
			}

			return targetObject.TrySetPropertyValue(propertyName, value);
		}
	}
}
