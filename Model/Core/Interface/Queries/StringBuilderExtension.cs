namespace Facton.ServiceApi.Domain.Model.Core.Queries
{
	using System.Text;

	public static class StringBuilderExtension
	{
		public static void AppendWithComma(this StringBuilder builder, string text)
		{
			if (builder.Length > 0)
			{
				builder.Append(", ");
			}

			builder.Append(text);
		}

		public static void AppendWithAnd(this StringBuilder builder, string text)
		{
			if (builder.Length > 0)
			{
				builder.Append(" AND ");
			}

			builder.Append(text);
		}
	}
}
