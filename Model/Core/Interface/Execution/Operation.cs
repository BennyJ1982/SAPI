namespace Facton.ServiceApi.Domain.Model.Core.Execution
{
	using System;

	[Flags]
	public enum Operation
	{
		None = 0,
		Get = 1,
		Post = 2,
		Patch = 4,
		Delete = 8,
		Put = 16
	}
}
